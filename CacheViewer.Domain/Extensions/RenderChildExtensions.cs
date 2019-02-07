namespace CacheViewer.Domain.Extensions
{
    using System;
    using Data.Entities;
    using Factories;

    public static class RenderChildExtensions
    {
        public const int UNDER1000_RANGE = 200;
        public const int OVER1000_RANGE = 5000;

        public static bool IsValidId(this RenderChild renderChild)
        {
            if (!RenderInformationFactory.Instance.IsValidRenderId(renderChild.ChildRenderId))
            {
                return false;
            }
            int range = renderChild.ChildRenderId > renderChild.ParentId ?
                Math.Abs(renderChild.ChildRenderId - renderChild.ParentId) :
                Math.Abs(renderChild.ParentId - renderChild.ChildRenderId);

            if (renderChild.ParentId < 1000)
            {
                return range < UNDER1000_RANGE;
            }

            return range < OVER1000_RANGE;
        }
    }
}