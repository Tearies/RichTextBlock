using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace RichTextBlock.Control
{
    public class RichTextRuleSource : ObservableCollection<RichTextRule>
    {
        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class.</summary>
        public RichTextRuleSource()
        {
            
        }

        

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified list.</summary>
        /// <param name="list">The list from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="list" /> parameter cannot be <see langword="null" />.</exception>
        public RichTextRuleSource(List<RichTextRule> list) : base(list)
        {
        }

        /// <summary>Initializes a new instance of the <see cref="T:System.Collections.ObjectModel.ObservableCollection`1" /> class that contains elements copied from the specified collection.</summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="collection" /> parameter cannot be <see langword="null" />.</exception>
        public RichTextRuleSource(IEnumerable<RichTextRule> collection) : base(collection)
        {
        }

        

        public new void Add(RichTextRule item)
        {  
           base.Add(item);
        }

        public new void Clear()
        {
           base.Clear();
        }
         
        public new bool Remove(RichTextRule item)
        {
            return base.Remove(item);
        }
 
    }
}