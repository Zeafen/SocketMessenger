using Primary_Massager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Primary_Massager.Comands
{
    public class EditCommand : ModifyingComandsBase
    {
        public override bool CanExecute(object? parameter) => parameter != null&&parameter is Message;

        public override void Execute(object? parameter)
        {
            (parameter as Message).IsEditing = true;
        }
    }
}
