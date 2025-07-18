using System.Runtime.CompilerServices;
using FluentAssertions;
using Sinter.Core;

namespace Sinter.Tests.Units;

public class UnitTests
{
    [Fact]
    public void Unit_Value_ReturnSameReference()
    {
        // Act
        ref readonly var value1 = ref Unit.Value;
        ref readonly var value2 = ref Unit.Value;
        
        // Assert
        Unsafe.AreSame(ref Unsafe.AsRef(in value1), ref Unsafe.AsRef(in value2))
            .Should()
            .BeTrue();
    }

    [Fact]
    public void Unit_Equals_AlwaysReturnsTrue()
    {
        // Arrange
        var unit1 = Unit.Value;
        var unit2 = new Unit();
        
        // Act & Assert
        unit1.Equals(unit2).Should().BeTrue();
        unit1.Equals((object)unit2).Should().BeTrue();
        (unit1 == unit2).Should().BeTrue();
        (unit1 != unit2).Should().BeFalse();
    }

    [Fact]
    public void Unit_GetHashCode_AlwaysReturnsZero()
    {
        // Act & Assert
        Unit.Value.GetHashCode().Should().Be(0);
    }

    [Fact]
    public void Unit_ToString_ReturnsParentheses()
    {
        Unit.Value.ToString().Should().Be("()");
    }
    
    [Fact]
    public void Unit_CompareTo_AlwaysReturnsZero()
    {
        // Arrange
        var unit1 = Unit.Value;
        var unit2 = new Unit();
        
        // Act & Assert
        unit1.CompareTo(unit2).Should().Be(0);
        unit1.CompareTo((object)unit2).Should().Be(0);
        unit1.CompareTo(null).Should().Be(0);
    }
}