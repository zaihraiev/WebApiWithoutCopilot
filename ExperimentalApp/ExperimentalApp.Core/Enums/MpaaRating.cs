using NpgsqlTypes;

namespace ExperimentalApp.Core.Enums
{
    /// <summary>
    /// Represents enumeratin from database for rating in film
    /// </summary>
    public enum MPAA_Rating
    {
        [PgName("G")]
        G,
        [PgName("PG")]
        PG,
        [PgName("PG13")]
        PG13,
        [PgName("R")]
        R,
        [PgName("NC17")]
        NC17
    }
}
