using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace HouseRentingSystem.ModelBinders
{
    public class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            ValueProviderResult valueResult = bindingContext.ValueProvider
                .GetValue(bindingContext.ModelName);

            if (valueResult != ValueProviderResult.None && !string.IsNullOrEmpty(valueResult.FirstValue))
            {
                decimal actualValue = 0M;
                bool success = false;

                try
                {
                    string decimalValue = valueResult.FirstValue;
                    decimalValue = decimalValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    decimalValue = decimalValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    actualValue = Convert.ToDecimal(decimalValue, CultureInfo.CurrentCulture);
                    success = true;
                }
                catch (FormatException fe)
                {
                    // Microsoft's siggestio is to add the whole exception object, the ModelState then extracts the error message
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, fe, bindingContext.ModelMetadata);
                }

                if (success)
                {
                    bindingContext.Result = ModelBindingResult.Success(actualValue);
                }
            }

            // If binding cannot occur, no error is thrown (the default behavior of the model binders)
            // no ModelState error is added
            return Task.CompletedTask;
        }
    }
}
