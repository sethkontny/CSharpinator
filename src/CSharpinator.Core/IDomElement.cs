﻿using System.Collections.Generic;

namespace CSharpinator
{
    public interface IDomElement
    {
        bool HasElements { get; }
        IEnumerable<IDomElement> Elements { get; }
        bool ActsAsRootElement { get; }
        Property CreateProperty(IClassRepository classRepository);
        DomPath GetDomPath(IFactory factory);
    }
}
