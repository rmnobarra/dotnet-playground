using System;
using System.Windows.Forms;
using CalculatorLib;

namespace CalculatorApp
{
    public partial class MainForm : Form
    {
        private Calculator calculator = new Calculator();

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            PerformOperation(calculator.Add);
        }

        private void btnSubtract_Click(object sender, EventArgs e)
        {
            PerformOperation(calculator.Subtract);
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            PerformOperation(calculator.Multiply);
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            PerformOperation(calculator.Divide);
        }

        private void PerformOperation(Func<double, double, double> operation)
        {
            try
            {
                double a = double.Parse(txtOperand1.Text);
                double b = double.Parse(txtOperand2.Text);
                double result = operation(a, b);
                txtResult.Text = result.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
