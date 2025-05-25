namespace Cezzi.Applications.Tests.Validators;

using Cezzi.Applications.Validators;
using FluentAssertions;
using Xunit;

public class EmailValidatorTests
{
    [Theory]
    [InlineData("r@cezzis.com")]
    [InlineData("r@gmail.edu.com")]
    [InlineData("r+something@gmail.com")]
    [InlineData("r+1@gmail.com")]
    [InlineData("r-something@gmail.com")]
    [InlineData("r-Something@gmail.com")]
    [InlineData("003676945@coyote.csusb.edu")]
    [InlineData("000@gmail.com")]
    [InlineData("hcs2@g.clemson.edu")]
    [InlineData("HCurrent@scu.edu")]
    [InlineData("HCurrent@scu.EDU")]
    [InlineData("H@EEDU.COM")]
    [InlineData("hcwatk01@cardmail.louisville.edu")]
    [InlineData("h-0@cardmail.louisville.edu")]
    [InlineData("WARRENANDBETSY@GMAIL.COM")]
    [InlineData("Annelise.r.l@gmail.com")]
    [InlineData("i@g.cn")]
    [InlineData("i-@s.cn")]
    [InlineData("-1@s.cn")]
    [InlineData("-1@A.cn")]
    [InlineData("-7@A.CN")]
    [InlineData("_@A.CN")]
    [InlineData("simple@example.com")]
    [InlineData("very.common@example.com")]
    [InlineData("disposable.style.email.with+symbol@example.com")]
    [InlineData("other.email-with-hyphen@example.com")]
    [InlineData("fully-qualified-domain@example.com")]
    [InlineData("user.name+tag+sorting@example.com")]
    [InlineData("x@example.com")]
    [InlineData("example-indeed@strange-example.com")]
    [InlineData("test/test@test.com")]
    [InlineData("example@s.example")]
    [InlineData("postmaster@[123.123.123.123]")]
    [InlineData("-@x.com")]
    [InlineData("a@-.com")]
    [InlineData("user-@example.org")]
    [InlineData(" r@gmail.com ")]
    public void email___success(string email)
    {
        var result = EmailValidator.Validate(email);
        result.Should().BeTrue();
    }

    [Theory]
    [InlineData("ron")]
    [InlineData(".")]
    [InlineData("r@")]
    [InlineData("r@x")]
    [InlineData("Abc.example.com")]
    [InlineData("A@b@c@example.com")]
    [InlineData(@"just""not""right@example.com")]
    [InlineData(@"this is""not\allowed@example.com")]
    [InlineData(@"this\ still\""not\\allowed@example.com")]
    [InlineData("QA[icon]CHOCOLATE[icon]@test.com")]
    [InlineData("admin@mailserver1")]
    [InlineData("postmaster@[IPv6:2001:0db8:85a3:0000:0000:8a2e:0370:7334]")]
    [InlineData("mailhost!username @example.org")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void email___failed(string email)
    {
        var result = EmailValidator.Validate(email);
        result.Should().BeFalse();
    }
}
