const tasksEl = document.getElementById("tasks");
const notificationsEl = document.getElementById("notifications");
const toastContainer = document.getElementById("toastContainer");
const connStatus = document.getElementById("connStatus");
const jwtInput = document.getElementById("jwt");
const connectBtn = document.getElementById("connectBtn");

// In-memory stores (ephemeral)
let tasksById = new Map();
let connection = null;

function setStatus(text) {
    connStatus.textContent = text;
}

function escapeHtml(s) {
    return String(s ?? "")
        .replaceAll("&", "&amp;")
        .replaceAll("<", "&lt;")
        .replaceAll(">", "&gt;")
        .replaceAll('"', "&quot;")
        .replaceAll("'", "&#039;");
}

function fmtDate(d) {
    try {
        return new Date(d).toLocaleString();
    } catch {
        return "";
    }
}

function renderTasks() {
    const tasks = Array.from(tasksById.values())
        .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));

    tasksEl.innerHTML = "";

    if (tasks.length === 0) {
        tasksEl.innerHTML = `<div class="muted" style="margin-top:10px;">No tasks found.</div>`;
        return;
    }

    for (const t of tasks) {
        const div = document.createElement("div");
        div.className = "task";

        div.innerHTML = `
      <div>
        <b>#${t.id}</b> ${escapeHtml(t.taskName)}
        <span class="badge">${escapeHtml(t.taskStatus)}</span>
      </div>
      <div style="margin-top:6px;">${escapeHtml(t.taskDescription)}</div>
      <div class="meta">
        Created: ${fmtDate(t.createdAt)}
        | Deadline: ${fmtDate(t.deadLine)}
        | Project: ${escapeHtml(t.project?.projectName ?? ("#" + (t.project?.id ?? "")))}
      </div>
    `;

        tasksEl.appendChild(div);
    }
}

function addNotification(task, kind) {
    const div = document.createElement("div");
    div.className = "notif";

    div.innerHTML = `
    <div><b>${escapeHtml(kind)}</b></div>
    <div style="margin-top:6px;">
      ${escapeHtml(task.taskName)} <span class="badge">${escapeHtml(task.taskStatus)}</span>
    </div>
    <div class="meta">${new Date().toLocaleString()}</div>
  `;

    // newest on top
    notificationsEl.prepend(div);
}

function showToast(task, kind) {
    const toast = document.createElement("div");
    toast.className = "toast";

    toast.innerHTML = `
    <div class="toastTitle">${escapeHtml(kind)}</div>
    <div>${escapeHtml(task.taskName)}</div>
    <div class="toastSmall">Status: ${escapeHtml(task.taskStatus)} • ${new Date().toLocaleString()}</div>
  `;

    toastContainer.appendChild(toast);

    // auto remove
    setTimeout(() => {
        toast.remove();
    }, 4000);
}

async function fetchMyTasks(token) {
    const res = await fetch("/api/ProjectTasks/my", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (!res.ok) {
        const text = await res.text();
        throw new Error(`GET /api/ProjectTasks/my failed: ${res.status} ${text}`);
    }

    const data = await res.json();
    tasksById = new Map(data.map(t => [t.id, t]));
    renderTasks();
}

function detectChangeKind(oldTask, newTask) {
    if (!oldTask) return "New task assigned";
    if (oldTask.taskStatus !== newTask.taskStatus) return "Task status updated";
    if (oldTask.taskName !== newTask.taskName || oldTask.taskDescription !== newTask.taskDescription) return "Task updated";
    if (oldTask.deadLine !== newTask.deadLine) return "Task deadline updated";
    return "Task updated";
}

async function connectSignalR(token) {
    if (connection) {
        try { await connection.stop(); } catch { }
        connection = null;
    }

    connection = new signalR.HubConnectionBuilder()
        .withUrl("/hubs/notifications", {
            accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build();

    connection.onreconnecting(() => setStatus("reconnecting"));
    connection.onreconnected(() => setStatus("connected"));
    connection.onclose(() => setStatus("disconnected"));

    connection.on("taskUpdated", (task) => {
        // Decide notification type (so it doesn't always say the same thing)
        const oldTask = tasksById.get(task.id);
        const kind = detectChangeKind(oldTask, task);

        // Update list
        tasksById.set(task.id, task);
        renderTasks();

        // Show notifications
        addNotification(task, kind);
        showToast(task, kind);

        console.log("taskUpdated received:", task);
    });

    await connection.start();
    setStatus("connected");
}

connectBtn.addEventListener("click", async () => {
    const token = jwtInput.value.trim();
    if (!token) {
        alert("Paste JWT token first (without 'Bearer ').");
        return;
    }

    setStatus("loading");

    try {
        await fetchMyTasks(token);
        await connectSignalR(token);
    } catch (e) {
        console.error(e);
        setStatus("error");
        alert(e.message);
    }
});