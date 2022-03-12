using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autocli
{
    internal static class Definitions
    {
        internal static Option? MakeOption<T>(
            Command? command,
            bool required,
            string name,
            string defaultvalue,
            string alias,
            string description)
        {
            try
            {
                if (command != null)
                {
                    Option<T>? option = new(name);
                    option.SetDefaultValue(defaultvalue);
                    option.Description = description;
                    option.AddAlias(alias);
                    option.IsRequired = required;
                    command.AddOption(option);
                    return option;
                }

                return null;
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("Input Command is null");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
        }

        internal static Argument? MakeArgument<T>(
            Command? command,
            string name,
            string defaultvalue,
            string description)
        {
            try
            {
                if (command != null)
                {
                    Argument<T>? argument = new(name);
                    argument.Description = description;
                    argument.SetDefaultValue(defaultvalue);
                    command.AddArgument(argument);
                    return argument;
                }
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("Input Command is null");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return null;
        }

        internal static Command? MakeCommand(
            RootCommand? rootcommand,
            string name,
            string description)
        {
            Command? command = new(name);
            try
            {
                if (rootcommand != null)
                {
                    command.Description = description;
                    rootcommand.AddCommand(command);
                    return command;
                }
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("RootCommand is null here");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return null;
        }
    }
}