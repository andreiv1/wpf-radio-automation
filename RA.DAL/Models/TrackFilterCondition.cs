using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RA.DAL.Models
{
    public enum FilterLabelType
    {
        Album,
        Category,
        DateAdded,
        DateModified,
        Duration,
        ReleaseDate,
        Status,
        Type,
        Title
    }

    public enum FilterOperator
    {
        Equals,
        LessThan,
        GreaterThan,
        Like
    }
    public class TrackFilterCondition
    {
        private FilterLabelType labelType;
        private FilterOperator filterOperator;
        private object? value;

        public FilterLabelType FilterLabelType { get { return labelType; } }
        public FilterOperator FilterOperator { get { return filterOperator; } }
        public object? Value { get { return value; } }

        public TrackFilterCondition(FilterLabelType labelType, FilterOperator filterOperator, object? value)
        {
            this.labelType = labelType;
            this.filterOperator = filterOperator;
            this.value = value;
        }
    }
}
