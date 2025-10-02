using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Management;
using System.Net.NetworkInformation;
using System.Reflection;
using RunDog.Resources;

namespace RunDog;

public partial class StatsForm : Form
{
    private readonly System.Windows.Forms.Timer? _updateTimer;
    private readonly PerformanceCounter? _cpuCounter;
    private readonly PerformanceCounter? _ramCounter;

    // --- Contrôles et données pour l'affichage ---
    private Label lblCpuTitle = new();
    private Label lblRamTitle = new();
    private Sparkline sparkCpu = new();
    private Sparkline sparkRam = new();
    private Label lblNetworkDetails = new();
    private string _currentIpAddress = "N/A";

    private List<Label> _diskTitles = new();
    private List<Label> _diskDetails = new();
    private List<Panel> _diskBars = new();
    private List<DriveInfo> _drives = new();

    public StatsForm()
    {
        try
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _ramCounter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            _cpuCounter?.NextValue();
            _ramCounter?.NextValue();
        }
        catch (Exception ex) { Console.WriteLine($"{RunDog.Resources.Resources.ErrorPerfCounters}: {ex.Message}"); }

        this.FormBorderStyle = FormBorderStyle.None;
        this.ShowInTaskbar = false;
        this.BackColor = Color.FromArgb(45, 45, 48);
        this.Padding = new Padding(1);
        this.Width = 320;
        this.AutoSize = true;
        this.AutoSizeMode = AutoSizeMode.GrowAndShrink;

        var mainLayout = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        mainLayout.Paint += (s, e) => { e.Graphics.DrawRectangle(new Pen(Color.FromArgb(60, 60, 60)), e.ClipRectangle); };
        this.Controls.Add(mainLayout);

        mainLayout.Controls.Add(CreateSection("RunDog.icons.cpu.png", lblCpuTitle, sparkCpu), 0, 0);
        mainLayout.Controls.Add(CreateSection("RunDog.icons.ram_memory.png", lblRamTitle, sparkRam), 0, 1);

        _drives = DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed).ToList();
        int currentRow = 2;
        foreach (var drive in _drives)
        {
            var title = new Label();
            var details = new Label();
            var bar = new Panel();
            _diskTitles.Add(title);
            _diskDetails.Add(details);
            _diskBars.Add(bar);
            mainLayout.Controls.Add(CreateStorageSection(drive, title, details, bar), 0, currentRow++);
        }

        mainLayout.Controls.Add(CreateNetworkSection(), 0, currentRow);

        _updateTimer = new System.Windows.Forms.Timer { Interval = 1000 };
        _updateTimer.Tick += (s, e) => UpdateStats();
        _updateTimer.Start();
        UpdateStats();

        this.Disposed += OnFormDisposed;
    }

    private Control CreateSection(string iconResource, Label titleLabel, Control content)
    {
        var rowLayout = new TableLayoutPanel { AutoSize = true, Margin = new Padding(10), ColumnCount = 2 };
        rowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 38));
        rowLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 260)); // Largeur fixe pour le contenu

        var icon = new PictureBox { Size = new Size(28, 28), SizeMode = PictureBoxSizeMode.Zoom, Image = GetIcon(iconResource), Margin = new Padding(0,0,10,0) };
        rowLayout.Controls.Add(icon, 0, 0);
        rowLayout.SetRowSpan(icon, 2);

        titleLabel.ForeColor = Color.White;
        titleLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        titleLabel.AutoSize = true;
        rowLayout.Controls.Add(titleLabel, 1, 0);

        content.Dock = DockStyle.Fill;
        content.Height = 30;
        rowLayout.Controls.Add(content, 1, 1);

        return rowLayout;
    }
    
    private Control CreateStorageSection(DriveInfo drive, Label titleLabel, Label detailsLabel, Panel progressBar)
    {
        var content = new TableLayoutPanel { Dock = DockStyle.Fill, AutoSize = true, RowCount = 2 };
        var barBackground = new Panel { BackColor = Color.FromArgb(60, 60, 60), Height = 8, Dock = DockStyle.Fill, Margin = new Padding(0, 4, 0, 4) };
        progressBar.BackColor = Color.Orange;
        progressBar.Height = 8;
        barBackground.Controls.Add(progressBar);
        content.Controls.Add(barBackground, 0, 0);

        detailsLabel.ForeColor = Color.Gray;
        detailsLabel.Font = new Font("Segoe UI", 8F);
        detailsLabel.AutoSize = true;
        content.Controls.Add(detailsLabel, 0, 1);

        var section = CreateSection("RunDog.icons.disque_storage.png", titleLabel, content);
        section.Tag = drive; // Store the DriveInfo in the Tag property

        // Attach click event to the section and all its child controls
        section.Click += OnStorageSectionClick;
        foreach (Control control in section.Controls)
        {
            control.Click += OnStorageSectionClick;
            // If the child control has its own children, attach the event to them too
            foreach (Control grandChild in control.Controls)
            {
                grandChild.Click += OnStorageSectionClick;
            }
        }
        titleLabel.Click += OnStorageSectionClick;
        detailsLabel.Click += OnStorageSectionClick;

        return section;
    }

    private void OnStorageSectionClick(object? sender, EventArgs e)
    {
        Control? clickedControl = sender as Control;
        DriveInfo? drive = clickedControl?.Tag as DriveInfo;

        if (drive != null && drive.IsReady)
        {
            try
            {
                Process.Start("explorer.exe", drive.RootDirectory.FullName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{RunDog.Resources.Resources.ErrorOpeningExplorer}: {ex.Message}");
            }
        }
    }


    private Control CreateNetworkSection()
    {
        var titleLabel = new Label { Text=RunDog.Resources.Resources.NetworkTitle, AutoSize=true, ForeColor=Color.White, Font = new Font("Segoe UI", 9F, FontStyle.Bold) };
        lblNetworkDetails.ForeColor = Color.Gray;
        lblNetworkDetails.Font = new Font("Segoe UI", 8F);
        lblNetworkDetails.AutoSize = true;
        
        var section = CreateSection("RunDog.icons.connexion_network.png", titleLabel, lblNetworkDetails);
        
        // Ajout du gestionnaire de clic à la section et à ses enfants
        section.Click += OnNetworkSectionClick;
        foreach(Control control in section.Controls) { control.Click += OnNetworkSectionClick; }
        titleLabel.Click += OnNetworkSectionClick;
        lblNetworkDetails.Click += OnNetworkSectionClick;

        return section;
    }

    private void OnNetworkSectionClick(object? sender, EventArgs e)
    {
        if (_currentIpAddress != "N/A")
        {
            Clipboard.SetText(_currentIpAddress);
            
            // Feedback visuel
            var originalText = lblNetworkDetails.Text;
            lblNetworkDetails.Text = $"{RunDog.Resources.Resources.LocalIp}: {RunDog.Resources.Resources.Copied}";
            var t = new System.Windows.Forms.Timer { Interval = 1500 };
            t.Tick += (s, args) => {
                lblNetworkDetails.Text = originalText;
                t.Stop();
                t.Dispose();
            };
            t.Start();
        }
    }

    private void UpdateStats()
    {
        float? cpuUsage = _cpuCounter?.NextValue();
        lblCpuTitle.Text = $"{RunDog.Resources.Resources.CpuTitle}: {cpuUsage ?? 0:F0} %";
        sparkCpu.AddValue(cpuUsage ?? 0);

        float? ramUsage = _ramCounter?.NextValue();
        lblRamTitle.Text = $"{RunDog.Resources.Resources.RamTitle}: {ramUsage ?? 0:F0} %";
        sparkRam.AddValue(ramUsage ?? 0);
        sparkRam.LineColor = Color.MediumPurple;

        for (int i = 0; i < _drives.Count; i++)
        {
            try
            {
                var drive = _drives[i];
                var usedDiskGb = (drive.TotalSize - drive.TotalFreeSpace) / 1073741824.0;
                var totalDiskGb = drive.TotalSize / 1073741824.0;
                var diskUsage = (usedDiskGb / totalDiskGb * 100.0);
                _diskTitles[i].Text = $"{RunDog.Resources.Resources.StorageTitle} ({drive.Name.Replace("\\", "")}{drive.VolumeLabel}): {diskUsage:F0} % {RunDog.Resources.Resources.Used}";
                _diskDetails[i].Text = $"{usedDiskGb:F1} GB / {totalDiskGb:F1} GB";
                UpdateBar(_diskBars[i], (float)diskUsage);
            }
            catch { _diskTitles[i].Text = $"{RunDog.Resources.Resources.StorageTitle}: N/A"; }
        }

        var (activeInterface, ipAddress) = GetActiveNetworkInterface();
        _currentIpAddress = ipAddress; // Stockage de l'IP
        lblNetworkDetails.Text = $"{activeInterface}\n{RunDog.Resources.Resources.LocalIp}: {ipAddress}";
    }

    private void UpdateBar(Panel bar, float? percentage) { if(bar.Parent != null) bar.Width = (int)(bar.Parent.Width * (percentage ?? 0) / 100.0f); }
    private Image? GetIcon(string resourceName) { try { var assembly = Assembly.GetExecutingAssembly(); using (Stream stream = assembly.GetManifestResourceStream(resourceName)!) { return Image.FromStream(stream); } } catch { return null; } }

    private (string, string) GetActiveNetworkInterface()
    {
        try
        {
            var firstUpInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(ni => ni.OperationalStatus == OperationalStatus.Up && (ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 || ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet) && ni.GetIPProperties().GatewayAddresses.Any());
            if (firstUpInterface != null)
            {
                var ip = firstUpInterface.GetIPProperties().UnicastAddresses.FirstOrDefault(ip => ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.Address.ToString() ?? "N/A";
                return (firstUpInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ? "Wi-Fi" : "Ethernet", ip);
            }
        }
        catch { }
        return ("N/A", "N/A");
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        var screen = Screen.FromControl(this) ?? Screen.PrimaryScreen;
        if (screen != null)
        {
            var workingArea = screen.WorkingArea;
            this.Location = new Point(workingArea.Right - this.Width - 10, workingArea.Bottom - this.Height);
        }
    }
    protected override void OnDeactivate(EventArgs e) { base.OnDeactivate(e); this.Close(); }
    private void OnFormDisposed(object? sender, EventArgs e) { _cpuCounter?.Dispose(); _ramCounter?.Dispose(); _updateTimer?.Dispose(); }
}