using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class MiniController : Form
{
    [DllImport("user32.dll")]
    private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

    private const byte VK_MEDIA_NEXT_TRACK = 0xB0;
    private const byte VK_MEDIA_PREV_TRACK = 0xB1;
    private const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
    private const byte VK_VOLUME_MUTE = 0xAD;
    private const byte VK_VOLUME_DOWN = 0xAE;
    private const byte VK_VOLUME_UP = 0xAF;

    private static void SendKey(byte vk)
    {
        keybd_event(vk, 0, 0, UIntPtr.Zero);
        keybd_event(vk, 0, 2, UIntPtr.Zero);
    }

    public MiniController()
    {
        // Ultra-minimal window with no branding
        this.Text = "";
        this.ClientSize = new Size(216, 76);
        this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
        this.StartPosition = FormStartPosition.CenterScreen;
        this.TopMost = true;
        this.BackColor = Color.FromArgb(24, 24, 24);
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        // Row 1: Playback Controls (⏮ | ▶/⏸ | ⏭)
        CreateButton("⏮", 8, 8, 62, 28, (s, e) => SendKey(VK_MEDIA_PREV_TRACK));
        
        Button btnPlay = CreateButton("▶ / ⏸", 76, 8, 64, 28, (s, e) => SendKey(VK_MEDIA_PLAY_PAUSE));
        btnPlay.BackColor = Color.FromArgb(50, 50, 50);
        btnPlay.Font = new Font("Segoe UI", 9f, FontStyle.Bold);

        CreateButton("⏭", 146, 8, 62, 28, (s, e) => SendKey(VK_MEDIA_NEXT_TRACK));

        // Row 2: Volume Controls (🔉 | 🔇 | 🔊)
        CreateButton("🔉 -", 8, 42, 62, 26, (s, e) => {
            SendKey(VK_VOLUME_DOWN);
            SendKey(VK_VOLUME_DOWN);
        });

        CreateButton("🔇", 76, 42, 64, 26, (s, e) => SendKey(VK_VOLUME_MUTE));

        CreateButton("🔊 +", 146, 42, 62, 26, (s, e) => {
            SendKey(VK_VOLUME_UP);
            SendKey(VK_VOLUME_UP);
        });
    }

    private Button CreateButton(string text, int x, int y, int width, int height, EventHandler onClick)
    {
        Button btn = new Button();
        btn.Text = text;
        btn.Location = new Point(x, y);
        btn.Size = new Size(width, height);
        btn.FlatStyle = FlatStyle.Flat;
        btn.FlatAppearance.BorderSize = 1;
        btn.FlatAppearance.BorderColor = Color.FromArgb(55, 55, 55);
        btn.BackColor = Color.FromArgb(35, 35, 35);
        btn.ForeColor = Color.White;
        btn.Font = new Font("Segoe UI", 8.5f, FontStyle.Regular);
        btn.Cursor = Cursors.Hand;
        btn.Click += onClick;
        this.Controls.Add(btn);
        return btn;
    }

    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MiniController());
    }
}
