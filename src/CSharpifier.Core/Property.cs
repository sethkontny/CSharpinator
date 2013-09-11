﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace CSharpifier
{
    public class Property
    {
        private readonly List<PropertyDefinition> _potentialPropertyDefinitions = new List<PropertyDefinition>();

        public Property(XName propertyName)
        {
            Name = new IdentifierName(propertyName.ToString());
        }

        public IdentifierName Name { get; set; }

        public IEnumerable<PropertyDefinition> PotentialPropertyDefinitions
        {
            get { return _potentialPropertyDefinitions; }
        }

        public PropertyDefinition SelectedPropertyDefinition
        {
            get { return _potentialPropertyDefinitions.First(x => x.IsEnabled && x.IsLegal); }
        }

        public void PrependPotentialPropertyDefinition(PropertyDefinition potentialPropertyDefinition)
        {
            InsertPotentialPropertyDefinition(0, potentialPropertyDefinition);
        }

        public void AppendPotentialPropertyDefinition(PropertyDefinition potentialPropertyDefinition)
        {
            InsertPotentialPropertyDefinition(_potentialPropertyDefinitions.Count, potentialPropertyDefinition);
        }

        private void InsertPotentialPropertyDefinition(int index, PropertyDefinition potentialPropertyDefinition)
        {
            var matchingPropertyDefinition = _potentialPropertyDefinitions.FirstOrDefault(x => Equals(x.Class, potentialPropertyDefinition.Class));
            if (matchingPropertyDefinition != null)
            {
                // If the the new property definition is illegal, its match should be illegal.
                // But not the other way around - if the new one is legal, and its match is
                // illegal, *don't* make the match legal. Note that we never want to change
                // IsEnabled - it's set purely by the user.
                if (!potentialPropertyDefinition.IsLegal)
                {
                    matchingPropertyDefinition.IsLegal = false;
                }

                return;
            }

            _potentialPropertyDefinitions.Insert(index, potentialPropertyDefinition);
        }

        public void AppendPotentialPropertyDefinitions(IEnumerable<PropertyDefinition> otherPotentialPropertyDefinitions)
        {
            foreach (var otherPotentialPropertyDefinition in otherPotentialPropertyDefinitions)
            {
                AppendPotentialPropertyDefinition(otherPotentialPropertyDefinition);
            }
        }

        public void PrependPotentialPropertyDefinitions(IEnumerable<PropertyDefinition> otherPotentialPropertyDefinitions)
        {
            foreach (var otherPotentialPropertyDefinition in otherPotentialPropertyDefinitions.Reverse())
            {
                PrependPotentialPropertyDefinition(otherPotentialPropertyDefinition);
            }
        }

        public string GeneratePropertyCode(Case classCase, Case propertyCase)
        {
            return SelectedPropertyDefinition.GeneratePropertyCode(classCase, propertyCase);
        }
    }
}
