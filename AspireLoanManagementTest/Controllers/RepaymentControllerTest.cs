using AspireLoanManagement.Business.Loan;
using AspireLoanManagement.Business.Models;
using AspireLoanManagement.Business.Repayment;
using AspireLoanManagement.Controllers;
using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AspireLoanManagementTest.Controllers
{
    public class RepaymentControllerTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<ILoanService> _loanServiceMock;
        private readonly Mock<IRepaymentService> _repaymentServiceMock;
        private readonly Mock<IValidator<RepaymentModelVM>> _repaymentValidatorMock;
        private readonly RepaymentController _repaymentController;

        public RepaymentControllerTest()
        {
            _fixture = new Fixture();
            _loanServiceMock = new Mock<ILoanService>();
            _repaymentServiceMock = new Mock<IRepaymentService>();
            _repaymentValidatorMock = new Mock<IValidator<RepaymentModelVM>>();
            _repaymentController = new RepaymentController(_repaymentServiceMock.Object);
        }

        [Fact]
        public async Task SettleRepayment_ValidModel_ReturnsTrue()
        {
            // Arrange
            var validRepaymentModel = _fixture.Create<RepaymentModelVM>();

            _repaymentValidatorMock.Setup(x => x.ValidateAsync(validRepaymentModel, default))
                .ReturnsAsync(new ValidationResult());

            var validationErrors = new ValidationResult
            {
                Errors = { new ValidationFailure("Property", "Error Message") }
            };
            _repaymentValidatorMock.Setup(x => x.ValidateAsync(It.IsAny<RepaymentModelVM>(), default))
                .ReturnsAsync(validationErrors);
            _repaymentServiceMock.Setup(x => x.SettleRepayment(It.IsAny<RepaymentModelVM>())).ReturnsAsync(true);

            // Act
            var result = await _repaymentController.SettleRepayment(validRepaymentModel);

            // Assert
            result.Should().BeTrue();
            _repaymentServiceMock.Verify(x => x.SettleRepayment(validRepaymentModel), Times.Once);
        }

        [Fact]
        public async Task SettleRepayment_InvalidModel_ThrowsException()
        {
            // Arrange
            var invalidRepaymentModel = _fixture.Build<RepaymentModelVM>().With(x => x.RepaymentAmount, 0).Create();

            ValidationResult validationErrors = new FluentValidation.Results.ValidationResult
            {
                Errors = { new ValidationFailure("Property", "Error Message") }
            };
            _repaymentValidatorMock.Setup(x => x.ValidateAsync(invalidRepaymentModel, default)).ReturnsAsync(validationErrors);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _repaymentController.SettleRepayment(invalidRepaymentModel));
            _repaymentServiceMock.Verify(x => x.SettleRepayment(It.IsAny<RepaymentModelVM>()), Times.Never);
        }

        [Fact]
        public async Task SettleRepayment_RepaymentServiceThrowsError()
        {
            // Arrange
            var validRepaymentModel = _fixture.Create<RepaymentModelVM>();

            _repaymentValidatorMock.Setup(x => x.ValidateAsync(validRepaymentModel, default))
                .ReturnsAsync(new ValidationResult());

            _repaymentServiceMock.Setup(x => x.SettleRepayment(validRepaymentModel))
                .ThrowsAsync(new Exception("Error message"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _repaymentController.SettleRepayment(validRepaymentModel));
            _repaymentValidatorMock.Verify();
            _loanServiceMock.Verify();
        }
    }
}
