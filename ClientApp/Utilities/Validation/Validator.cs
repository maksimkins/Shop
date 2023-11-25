using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Utilities.Validation;

public class Validator
{
    public static bool ValidateCredentials(string? username, string? password)
    {
        bool isInvalidUserName = string.IsNullOrWhiteSpace(username) || username.Length > 150;
        bool isInvalidPassword = string.IsNullOrWhiteSpace(password) || password.Length > 150;

        return !(isInvalidUserName || isInvalidPassword);
    }
}
