using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FocusTest
{
    public partial class MainForm : Form
    {
        private const uint EVENT_SYSTEM_FOREGROUND = 3;
        private const uint WINEVENT_OUTOFCONTEXT = 0;
        private const int WINEVENT_SKIPOWNPROCESS = 2;

        private readonly WinEventDelegate evDelegate = null;
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
            evDelegate = new WinEventDelegate(OnForegroundWindowChanged);
            eventHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, evDelegate, 0, 0, WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
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
            string name = process.ProcessName;
            if (string.IsNullOrWhiteSpace(process.MainWindowTitle) == false)
                name += $" - {process.MainWindowTitle}";

            var item = new ListViewItem(new string[] { name, process.Id.ToString(), DateTime.Now.ToLongTimeString() });
            listViewProcesses.Items.Insert(0, item);

            listViewProcesses.AutoResizeColumn(0, ColumnHeaderAutoResizeStyle.ColumnContent);
            listViewProcesses.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent);
        }
    }
}
