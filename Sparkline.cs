using System.Drawing.Drawing2D;

namespace RunDog;

public class Sparkline : Control
{
    private readonly List<float> _values = new();
    private const int MAX_VALUES = 50;

    public Color LineColor { get; set; } = Color.DodgerBlue;

    public Sparkline()
    {
        this.DoubleBuffered = true;
        this.Paint += Sparkline_Paint;
    }

    public void AddValue(float value)
    {
        _values.Add(Math.Clamp(value, 0, 100));
        if (_values.Count > MAX_VALUES)
        {
            _values.RemoveAt(0);
        }
        this.Invalidate(); // Redessine le contrôle
    }

    private void Sparkline_Paint(object? sender, PaintEventArgs e)
    {
        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        if (_values.Count < 2) return;

        using var pen = new Pen(LineColor, 2f);

        var points = new PointF[_values.Count];
        float maxVal = 100.0f; // On normalise sur 100%

        for (int i = 0; i < _values.Count; i++)
        {
            float x = (float)i / (MAX_VALUES - 1) * this.Width;
            float y = this.Height - (_values[i] / maxVal * this.Height);
            points[i] = new PointF(x, y);
        }

        e.Graphics.DrawLines(pen, points);
    }
}
