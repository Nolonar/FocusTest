using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FocusTest
{
    public partial class MainForm : Form
    {
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const uint WINEVENT_SKIPOWNPROCESS = 2;

        private readonly WinEventDelegate eventDelegate = null;
        private readonly IntPtr eventHook = IntPtr.Zero;

        [DllImport("user32.dll")]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);

        [DllImport("user32.dll")]
        private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

        [DllImport("user32.dll")]
        private static extern IntPtr UnhookWinEvent(IntPtr hWinEventHook);

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);

        public MainForm()
        {
            InitializeComponent();

            // Set eventDelegate scope outside of function to prevent garbage collection.
            eventDelegate = new WinEventDelegate(OnForegroundWindowChanged);

            // We'll need eventHook to unhook the event when we exit this app.
            eventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, eventDelegate, 0, 0, WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            listViewProcesses.Items.Clear();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnhookWinEvent(eventHook);
        }

        private void OnForegroundWindowChanged(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
        {
            GetWindowThreadProcessId(hwnd, out int foregroundProcessId);
            AddProcessToList(Process.GetProcessById(foregroundProcessId));
        }

        private void AddProcessToList(Process process)
        {
            string[] nameParts = { process.ProcessName, process.MainWindowTitle };
            string name = string.Join(" - ", nameParts.Where(p => !string.IsNullOrWhiteSpace(p)));

            var item = new ListViewItem(new string[] { name, process.Id.ToString(), DateTime.Now.ToLongTimeString() });
            listViewProcesses.Items.Insert(0, item);

            ResizeColumns();
        }

        private void ResizeColumns()
        {
            int[] colIndicesToResize = { 0, 1 };
            foreach (int colIndex in colIndicesToResize)
                listViewProcesses.AutoResizeColumn(colIndex, ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
