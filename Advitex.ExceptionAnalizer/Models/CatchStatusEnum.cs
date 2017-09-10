namespace Advitex.ExceptionAnalizer.Models
{
    /// <summary>
    /// Catch status of exceptions
    /// </summary>
    public enum CatchStatusEnum
    {
        Unknown = 0,
        NotCatched = 1,
        Catched = 2,
        CatchedInAllClause = 3,
    }
}