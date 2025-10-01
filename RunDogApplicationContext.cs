using System.Diagnostics;
using System.Runtime.InteropServices;
using RunDog.Resources;

namespace RunDog
{
    /// <summary>
    /// Contexte d'application principal qui gère le cycle de vie de l'icône dans la zone de notification.
    /// </summary>
    public class RunDogApplicationContext : ApplicationContext
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        extern static bool DestroyIcon(IntPtr handle);

        private readonly NotifyIcon _notifyIcon;
        private readonly System.Windows.Forms.Timer _animationTimer;
        private readonly PerformanceCounter _cpuCounter;

        private readonly List<SpriteSheet> _spriteSheets = new();
        private int _currentSpriteSheetIndex = 0;
        private int _currentFrame = 0;
        private bool _isPaused = false;
        private StatsForm? _statsForm; // Ajout pour la nouvelle fenêtre
        private ToolStripMenuItem _changeAnimalMenuItem; // Déclaration du champ

        private const int MIN_INTERVAL_MS = 20;
        private const int MAX_INTERVAL_MS = 200;

        public RunDogApplicationContext()
        {
            _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            _cpuCounter.NextValue();

            _spriteSheets.Add(new SpriteSheet("RunDog.animaux.rundog_animation_chien.png", 4, RunDog.Resources.Resources.DogAnimalName));
            _spriteSheets.Add(new SpriteSheet("RunDog.animaux.rundog_animation_cat.png", 4, RunDog.Resources.Resources.CatAnimalName));

            var contextMenu = new ContextMenuStrip();
            var pauseMenuItem = new ToolStripMenuItem(RunDog.Resources.Resources.PauseMenuItem, null, PauseOnClick);
            _changeAnimalMenuItem = new ToolStripMenuItem(RunDog.Resources.Resources.ChangeAnimalMenuItem);
            UpdateAnimalSubMenu(_changeAnimalMenuItem);
            contextMenu.Items.Add(pauseMenuItem);
            contextMenu.Items.Add(_changeAnimalMenuItem);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(RunDog.Resources.Resources.ExitMenuItem, null, ExitOnClick);

            _notifyIcon = new NotifyIcon()
            {
                Icon = _spriteSheets[_currentSpriteSheetIndex].Icons[0],
                Text = "CPU: 0.0%",
                Visible = true,
                ContextMenuStrip = contextMenu
            };
            _notifyIcon.MouseClick += OnNotifyIconClick; // Ajout du gestionnaire de clic

            _animationTimer = new System.Windows.Forms.Timer();
            _animationTimer.Tick += OnAnimationTimerTick;
            _animationTimer.Interval = MAX_INTERVAL_MS;
            _animationTimer.Start();
        }

        private void OnNotifyIconClick(object? sender, MouseEventArgs e)
        {
            // On ne réagit qu'au clic gauche
            if (e.Button != MouseButtons.Left) return;

            // Si la fenêtre est déjà ouverte, on la ferme.
            if (_statsForm != null && !_statsForm.IsDisposed)
            {
                _statsForm.Close();
                _statsForm = null;
            }
            else
            {
                // Sinon, on la crée et on l'affiche.
                _statsForm = new StatsForm();
                _statsForm.Show();
                _statsForm.Activate();
            }
        }

        private void UpdateAnimalSubMenu(ToolStripMenuItem parentMenu)
        {
            parentMenu.DropDownItems.Clear();
            foreach (var spriteSheet in _spriteSheets)
            {
                var menuItem = new ToolStripMenuItem(spriteSheet.Name, null, ChangeAnimalOnClick)
                {
                    Tag = _spriteSheets.IndexOf(spriteSheet),
                    Checked = (_spriteSheets.IndexOf(spriteSheet) == _currentSpriteSheetIndex)
                };
                parentMenu.DropDownItems.Add(menuItem);
            }
        }

        private void OnAnimationTimerTick(object? sender, EventArgs e)
        {
            if (_isPaused) return;
            float cpuUsage = _cpuCounter.NextValue();
            _notifyIcon.Text = $"CPU: {cpuUsage:F1}%";
            _currentFrame = (_currentFrame + 1) % _spriteSheets[_currentSpriteSheetIndex].Icons.Length;
            _notifyIcon.Icon = _spriteSheets[_currentSpriteSheetIndex].Icons[_currentFrame];
            double interval = MAX_INTERVAL_MS - (MAX_INTERVAL_MS - MIN_INTERVAL_MS) * (cpuUsage / 100.0);
            _animationTimer.Interval = (int)Math.Max(MIN_INTERVAL_MS, interval);
        }

        private void PauseOnClick(object? sender, EventArgs e)
        {
            _isPaused = !_isPaused;
            var menuItem = (ToolStripMenuItem)sender!;
            menuItem.Text = _isPaused ? RunDog.Resources.Resources.ResumeMenuItem : RunDog.Resources.Resources.PauseMenuItem;
        }

        private void ChangeAnimalOnClick(object? sender, EventArgs e)
        {
            var menuItem = (ToolStripMenuItem)sender!;
            _currentSpriteSheetIndex = (int)menuItem.Tag!;
            _currentFrame = 0;
            UpdateAnimalSubMenu(_changeAnimalMenuItem); // Mettre à jour le sous-menu après le changement d'animal
        }

        private void ExitOnClick(object? sender, EventArgs e)
        {
            _animationTimer.Stop();
            _notifyIcon.Visible = false;
            foreach (var spriteSheet in _spriteSheets)
            {
                foreach (var icon in spriteSheet.Icons)
                {
                    DestroyIcon(icon.Handle);
                    icon.Dispose();
                }
            }
            _cpuCounter.Dispose();
            _notifyIcon.Dispose();
            Application.Exit();
        }
    }
}