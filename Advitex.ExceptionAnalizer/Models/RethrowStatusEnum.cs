namespace Advitex.ExceptionAnalizer.Models
{
    public enum RethrowStatusEnum
    {
        None,
        RethrowNewExceptionWithInnerException,
        RethrowNewExceptionWithoutInnerException,
        RethrowWithCallStackData,
        RethrowWithoutCallStackData
    }
}