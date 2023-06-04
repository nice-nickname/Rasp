using System.Data;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using NHibernate;
using NHibernate.Engine;
using NHibernate.SqlTypes;
using NHibernate.UserTypes;

namespace Domain.Persistence.Mappers;

[ExcludeFromCodeCoverage]
public class NHibernateColorType : IUserType
{
    public bool Equals(object x, object y)
    {
        if (ReferenceEquals(x, y))
        {
            return true;
        }

        if (x == null || y == null)
        {
            return false;
        }

        return x.Equals(y);
    }

    public int GetHashCode(object? x)
    {
        return x == null ? typeof(Color).GetHashCode() + 473 : x.GetHashCode();
    }

    public object NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor session, object owner)
    {
        var obj = NHibernateUtil.String.NullSafeGet(rs, names, session, owner);
        if (obj == null)
        {
            return null;
        };

        return ColorTranslator.FromHtml((string)obj);
    }

    public void NullSafeSet(DbCommand cmd, object? value, int index, ISessionImplementor session)
    {
        if (value == null)
        {
            ((IDataParameter)cmd.Parameters[index]).Value = DBNull.Value;
        }
        else
        {
            ((IDataParameter)cmd.Parameters[index]).Value = ColorTranslator.ToHtml((Color)value);
        }
    }

    public object DeepCopy(object value)
    {
        return value;
    }

    public object Replace(object original, object target, object owner)
    {
        return original;
    }

    public object Assemble(object cached, object owner)
    {
        return cached;
    }

    public object Disassemble(object value)
    {
        return value;
    }

    public SqlType[] SqlTypes => new[]
    { 
            new SqlType(DbType.AnsiStringFixedLength),
    };

    public Type ReturnedType => typeof(Color);

    public bool IsMutable => true;
}
