using System;
using System.Drawing;
using System.Windows.Forms;

namespace Calucalor;

public partial class Form1 : Form
{
    private Label lblDisplay = new Label();
    private Label lblExpression = new Label();
    private double firstNumber = 0;
    private bool startNewNumber = true;
    private string currentOperator = "";
    private Panel buttonPanel = new Panel();

    public Form1()
    {
        InitializeComponent();
        CreateCalculatorUI();
    }

    private void CreateCalculatorUI()
    {
        this.Text = "Calculator";
        this.Size = new Size(400, 750);
        this.StartPosition = FormStartPosition.CenterScreen;
        this.BackColor = Color.White;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;

        Panel mainPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        Panel displayPanel = new Panel
        {
            Dock = DockStyle.Top,
            Height = 180,
            BackColor = Color.White,
            Padding = new Padding(20, 20, 20, 0)
        };

        lblExpression = new Label
        {
            Text = "",
            Dock = DockStyle.Top,
            Height = 40,
            TextAlign = ContentAlignment.BottomRight,
            Font = new Font("Segoe UI", 20),
            ForeColor = Color.FromArgb(120, 120, 120),
            BackColor = Color.White,
            Padding = new Padding(0, 0, 0, 10)
        };
        displayPanel.Controls.Add(lblExpression);

        lblDisplay = new Label
        {
            Text = "0",
            Dock = DockStyle.Top,
            Height = 100,
            TextAlign = ContentAlignment.BottomRight,
            Font = new Font("Segoe UI", 60),
            ForeColor = Color.Black,
            BackColor = Color.White,
            AutoSize = false
        };
        displayPanel.Controls.Add(lblDisplay);

        mainPanel.Controls.Add(displayPanel);

        buttonPanel = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White,
            Padding = new Padding(8)
        };

        CreateButtons();

        mainPanel.Controls.Add(buttonPanel);
        this.Controls.Add(mainPanel);
    }

    private void CreateButtons()
    {
        var buttonData = new (int row, int col, string text, string type, int colSpan)[]
        {
            (0, 0, "AC", "func", 0),
            (0, 1, "+/-", "func", 0),
            (0, 2, "%", "func", 0),
            (0, 3, "÷", "op", 0),
            (1, 0, "7", "num", 0),
            (1, 1, "8", "num", 0),
            (1, 2, "9", "num", 0),
            (1, 3, "×", "op", 0),
            (2, 0, "4", "num", 0),
            (2, 1, "5", "num", 0),
            (2, 2, "6", "num", 0),
            (2, 3, "-", "op", 0),
            (3, 0, "1", "num", 0),
            (3, 1, "2", "num", 0),
            (3, 2, "3", "num", 0),
            (3, 3, "+", "op", 0),
            (4, 0, "0", "num", 2),
            (4, 2, ".", "num", 0),
            (4, 3, "=", "op", 0),
        };

        foreach (var btn in buttonData)
        {
            CreateModernButton(btn.row, btn.col, btn.text, btn.type, btn.colSpan);
        }
    }

    private void CreateModernButton(int row, int col, string text, string type, int colSpan)
    {
        Button button = new Button
        {
            Text = text,
            Font = new Font("Segoe UI", 26),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand,
            Tag = type
        };

        Color bgColor;
        Color fgColor;
        if (type == "op")
        {
            bgColor = Color.FromArgb(0, 122, 255); // Blue
            fgColor = Color.White;
        }
        else if (type == "func")
        {
            bgColor = Color.FromArgb(200, 200, 200); // Medium gray
            fgColor = Color.Black;
        }
        else // num
        {
            bgColor = Color.FromArgb(240, 240, 240); // Light gray-white
            fgColor = Color.Black;
        }

        button.BackColor = bgColor;
        button.ForeColor = fgColor;

        button.FlatAppearance.BorderSize = 0;
        button.FlatAppearance.MouseOverBackColor = AddLight(bgColor, 30);
        button.FlatAppearance.MouseDownBackColor = AddLight(bgColor, -20);

        button.Click += Button_Click;

        int cellSize = 80;
        int margin = 6;
        int x = col * (cellSize + margin) + margin;
        int y = row * (cellSize + margin) + margin + 200; // Offset for display

        button.Location = new Point(x, y);
        button.Size = new Size(colSpan == 2 ? cellSize * 2 + margin : cellSize, cellSize);

        if (colSpan == 2)
            button.Location = new Point(x - margin, y);

        buttonPanel.Controls.Add(button);
    }

    private Color AddLight(Color color, int amount)
    {
        int r = Math.Min(255, Math.Max(0, color.R + amount));
        int g = Math.Min(255, Math.Max(0, color.G + amount));
        int b = Math.Min(255, Math.Max(0, color.B + amount));
        return Color.FromArgb(r, g, b);
    }

    private void Button_Click(object? sender, EventArgs e)
    {
        Button btn = (Button)sender!;
        string text = btn.Text;
        string type = btn.Tag?.ToString() ?? "num";

        if (type == "num" || text == ".")
        {
            if (text == ".")
            {
                if (!lblDisplay.Text.Contains("."))
                {
                    lblDisplay.Text = startNewNumber ? "0." : lblDisplay.Text + ".";
                    startNewNumber = false;
                }
            }
            else
            {
                if (startNewNumber)
                {
                    lblDisplay.Text = text;
                    startNewNumber = false;
                }
                else
                {
                    lblDisplay.Text = lblDisplay.Text == "0" ? text : lblDisplay.Text + text;
                }
            }
        }
        else if (type == "func")
        {
            if (text == "AC")
            {
                lblDisplay.Text = "0";
                lblExpression.Text = "";
                firstNumber = 0;
                currentOperator = "";
                startNewNumber = true;
            }
            else if (text == "+/-")
            {
                if (lblDisplay.Text != "0")
                {
                    if (lblDisplay.Text.StartsWith("-"))
                        lblDisplay.Text = lblDisplay.Text.Substring(1);
                    else
                        lblDisplay.Text = "-" + lblDisplay.Text;
                }
            }
            else if (text == "%")
            {
                double value = double.Parse(lblDisplay.Text) / 100;
                lblDisplay.Text = value.ToString("F2");
                startNewNumber = true;
            }
        }
        else if (type == "op")
        {
            if (text == "=")
            {
                if (currentOperator != "" && !startNewNumber)
                {
                    double secondNumber = double.Parse(lblDisplay.Text.Replace(",", "."));
                    double result = 0;

                    switch (currentOperator)
                    {
                        case "+": result = firstNumber + secondNumber; break;
                        case "-": result = firstNumber - secondNumber; break;
                        case "×": result = firstNumber * secondNumber; break;
                        case "÷": result = secondNumber != 0 ? firstNumber / secondNumber : 0; break;
                    }

                    lblExpression.Text = lblExpression.Text + lblDisplay.Text + " =";
                    lblDisplay.Text = FormatResult(result);
                    currentOperator = "";
                    startNewNumber = true;
                }
            }
            else
            {
                if (!startNewNumber && currentOperator != "")
                {
                    double secondNumber = double.Parse(lblDisplay.Text.Replace(",", "."));
                    double result = 0;

                    switch (currentOperator)
                    {
                        case "+": result = firstNumber + secondNumber; break;
                        case "-": result = firstNumber - secondNumber; break;
                        case "×": result = firstNumber * secondNumber; break;
                        case "÷": result = secondNumber != 0 ? firstNumber / secondNumber : 0; break;
                    }

                    firstNumber = result;
                    lblDisplay.Text = FormatResult(result);
                }
                firstNumber = double.Parse(lblDisplay.Text.Replace(",", "."));
                currentOperator = text;
                lblExpression.Text = lblDisplay.Text + " " + text;
                startNewNumber = true;
            }
        }
    }

    private string FormatResult(double value)
    {
        return value.ToString("0.##").Replace(",", ".");
    }
}
