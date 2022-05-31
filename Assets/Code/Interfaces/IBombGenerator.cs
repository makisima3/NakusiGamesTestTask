using System.Collections.Generic;
using DefaultNamespace;

namespace Code.Interfaces
{
    public interface IBombGenerator
    {
        List<IBomb> Generate(Cell[,] cells,int count);
    }
}