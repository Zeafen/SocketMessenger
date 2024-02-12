using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Primary_Massager.Rules
{
    public class NumInputRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
			int port = 0;
			try
			{
				port = int.Parse((string)value);
			}
			catch (Exception ex)
			{
				return new ValidationResult(false, $"Invalid characters or {ex.Message}");
				throw;
			}
			if (port < 0) return new ValidationResult(false, "Value cannot be less than zero");
			return new ValidationResult(true, null);
        }
    }
}
