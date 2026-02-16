using System.Text.Json;

namespace sample_app;

public partial class Form1 : Form
{
    private TextBox txtIpAddress;
    private TextBox txtPort;
    private Button btnSave;
    private Label lblIp;
    private Label lblPort;

    public Form1()
    {
        InitializeComponent();
        SetupCustomUi();
    }

    private void SetupCustomUi()
    {
        this.Text = "Sample App - IP/Port Saver";
        this.Size = new Size(300, 300);

        lblIp = new Label { Text = "IP Address:", Location = new Point(20, 20), AutoSize = true };
        txtIpAddress = new TextBox { Location = new Point(20, 40), Width = 240 };

        lblPort = new Label { Text = "Port:", Location = new Point(20, 70), AutoSize = true };
        txtPort = new TextBox { Location = new Point(20, 90), Width = 240 };

        btnSave = new Button { Text = "Save Settings", Location = new Point(20, 130), Width = 240 };
        btnSave.Click += BtnSave_Click;

        this.Controls.Add(lblIp);
        this.Controls.Add(txtIpAddress);
        this.Controls.Add(lblPort);
        this.Controls.Add(txtPort);
        this.Controls.Add(btnSave);
    }

    private void BtnSave_Click(object? sender, EventArgs e)
    {
        var settings = new
        {
            IpAddress = txtIpAddress.Text,
            Port = txtPort.Text
        };

        try
        {
            string jsonString = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "setting.json");
            File.WriteAllText(filePath, jsonString);
            MessageBox.Show($"Settings saved to:\n{filePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
