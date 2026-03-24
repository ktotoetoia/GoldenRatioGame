using UnityEngine;

namespace IM.Items
{
    [CreateAssetMenu(menuName = "Tags/LazyTag")]
    public class LazyTag : ScriptableObject, ITag
    {
        public virtual string TagName => name;
        
        public bool Matches(ITag other)
        {
            return other != null  && other.TagName == TagName;
        }
    }
}