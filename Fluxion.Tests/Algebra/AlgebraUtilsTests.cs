using Fluxion.Fluxion.Math;
using Fluxion.Math.Algebra;
using Xunit;

namespace Fluxion.Tests.Algebra
{
    public class AlgebraUtilsTests
    {
        [Fact]
        public void Discriminant_Works()
        {
            // x^2 - 3x + 2 -> Δ = 1
            var d = AlgebraUtils.Discriminant(1, -3, 2);
            Assert.Equal(1, d);
        }

        [Fact]
        public void NearlyEqual_Works()
        {
            Assert.True(AlgebraUtils.NearlyEqual(1.000000001, 1.0));
        }

        [Fact]
        public void Gcd_Works()
        {
            Assert.Equal(6, AlgebraUtils.GCD(54, 24));
        }
    }
}
