using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharacterMapExtension.CharMap;

public interface ISymbol
{
    string Symbol { get; }
    string Description { get; }
    string Category { get; }
    string Unicode { get; }
    string Dec { get; }
    string Latex { get; }
    List<string> Keywords { get; }
}
