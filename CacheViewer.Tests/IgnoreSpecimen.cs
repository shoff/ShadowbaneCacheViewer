namespace CacheViewer.Tests
{
    using System.Reflection;
    using AutoFixture.Kernel;

    public class IgnoreSpecimen<T> : ISpecimenBuilder
        where T : class
    {
        /// <summary>
        ///     Creates a new specimen based on a request.
        /// </summary>
        /// <param name="request">The request that describes what to create.</param>
        /// <param name="context">A context that can be used to create other specimens.</param>
        /// <returns>
        ///     The requested specimen if possible; otherwise a <see cref="T:Ploeh.AutoFixture.Kernel.NoSpecimen" /> instance.
        /// </returns>
        /// <remarks>
        ///     <para>
        ///         The <paramref name="request" /> can be any object, but will often be a
        ///         <see cref="T:System.Type" /> or other <see cref="T:System.Reflection.MemberInfo" /> instances.
        ///     </para>
        ///     <para>
        ///         Note to implementers: Implementations are expected to return a
        ///         <see cref="T:Ploeh.AutoFixture.Kernel.NoSpecimen" /> instance if they can't satisfy the request.
        ///     </para>
        /// </remarks>
        public object Create(object request, ISpecimenContext context)
        {
            var pi = request as PropertyInfo;
            if (pi != null && pi.PropertyType == typeof(T))
            {
                return new OmitSpecimen();
            }

            return new NoSpecimen();
        }
    }
}