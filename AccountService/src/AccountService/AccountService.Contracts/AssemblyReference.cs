using System.Reflection;

namespace AccountService.Contracts;
public static class AssemblyReference
{
    public static Assembly Assembly => typeof(Assembly).Assembly;
}
