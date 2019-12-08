using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace MyCalculatorv1
{
	public partial class MainWindow : Window, IComponentConnector
	{
		//internal TextBox tb;

		//private bool _contentLoaded;

		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click_1(object sender, RoutedEventArgs e)
		{
			Button button = (Button)sender;
			tb.Text += button.Content.ToString();
		}

		private void Result_click(object sender, RoutedEventArgs e)
		{
			result();
		}

        //private void result()
        //{
        //    int num = 0;
        //    if (tb.Text.Contains("+"))
        //    {
        //        num = tb.Text.IndexOf("+");
        //    }
        //    else if (tb.Text.Contains("-"))
        //    {
        //        num = tb.Text.IndexOf("-");
        //    }
        //    else if (tb.Text.Contains("*"))
        //    {
        //        num = tb.Text.IndexOf("*");
        //    }
        //    else if (tb.Text.Contains("/"))
        //    {
        //        num = tb.Text.IndexOf("/");
        //    }
        //    string a = tb.Text.Substring(num, 1);
        //    double num2 = Convert.ToDouble(tb.Text.Substring(0, num));

        //    Падает в этой строке потому что tb.Text - "11++++23" и падает exception при конверации "+++23" в число

        //    double num3 = Convert.ToDouble(tb.Text.Substring(num + 1, tb.Text.Length - num - 1));
        //    if (a == "+")
        //    {
        //        TextBox textBox = tb;
        //        textBox.Text = textBox.Text + "=" + (num2 + num3);
        //    }
        //    else if (a == "-")
        //    {
        //        TextBox textBox = tb;
        //        textBox.Text = textBox.Text + "=" + (num2 - num3);
        //    }
        //    else if (a == "*")
        //    {
        //        TextBox textBox = tb;
        //        textBox.Text = textBox.Text + "=" + num2 * num3;
        //    }
        //    else
        //    {
        //        TextBox textBox = tb;
        //        textBox.Text = textBox.Text + "=" + num2 / num3;
        //    }
        //}

		private void result()
		{
            string signs = "+-*/";
            string expression = tb.Text;
            int num = expression.IndexOfAny(signs.ToCharArray(), 1);
            if(num < 0)
            {
                MessageBox.Show($"Выражение '{tb.Text}' не содержит знак операции. Пример: 1+25 или 42/2");
                return;
            }

            char sign = expression[num];
            
            var numstr = expression.Substring(0, num);
            if (!double.TryParse(numstr, out double num1))
            {
                MessageBox.Show($"Неверный формат первого числа - '{numstr}'.");
                return;
            }

            numstr = expression.Substring(num + 1, expression.Length - num - 1);
            if (!double.TryParse(numstr, out double num2))
            {
                MessageBox.Show($"Неверный формат второго числа - '{numstr}'.");
                return;
            }

            switch (sign)
            {
                case '+':
                    tb.Text = expression + "=" + (num1 + num2);
                    break;
                case '-':
                    tb.Text = expression + "=" + (num1 - num2);
                    break;
                case '*':
                    tb.Text = expression + "=" + (num1 * num2);
                    break;
                case '/':
                    tb.Text = expression + "=" + (num1 / num2);
                    break;
                default:
                    break;
            } 
		}

		private void Off_Click_1(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void Del_Click(object sender, RoutedEventArgs e)
		{
			tb.Text = "";
		}

		private void R_Click(object sender, RoutedEventArgs e)
		{
			if (tb.Text.Length > 0)
			{
				tb.Text = tb.Text.Substring(0, tb.Text.Length - 1);
			}
		}
	}
}
