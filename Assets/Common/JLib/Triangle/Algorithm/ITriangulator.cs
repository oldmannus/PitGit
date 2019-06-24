// -----------------------------------------------------------------------
// <copyright file="ITriangulator.cs" company="">
// Triangle.NET code by Christian Woltering, http://triangle.codeplex.com/
// </copyright>
// -----------------------------------------------------------------------

namespace JLib.Utilities.Triangle.Algorithm
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface ITriangulator
    {
        int Triangulate(Mesh mesh);
    }
}
