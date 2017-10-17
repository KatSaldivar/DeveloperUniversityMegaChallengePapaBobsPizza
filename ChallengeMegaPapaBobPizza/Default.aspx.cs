using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PapaBobs.DTO.Enums;

namespace ChallengeMegaPapaBobPizza
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void orderButton_Click(object sender, EventArgs e)
        {
            if (nametextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a name.";
                validationLabel.Visible = true;
                return;
            }
            if(addressTextBox.Text.Trim().Length ==0)
            {
                validationLabel.Text = "Please enter an address.";
                validationLabel.Visible = true;
                return;
            }
            if (zipTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a zip code.";
                validationLabel.Visible = true;
                return;
            }
            if (phoneTextBox.Text.Trim().Length == 0)
            {
                validationLabel.Text = "Please enter a phone number.";
                validationLabel.Visible = true;
                return;
            }

            try
            {
                var order = buildOrder();
                PapaBobs.Domain.OrderManager.CreateOrder(order);
                Response.Redirect("success.aspx");
            }
            catch (Exception ex)
            {
                validationLabel.Text = ex.Message;
                validationLabel.Visible = true;
                return;
            }
            
        }

        private PapaBobs.DTO.Enums.PaymentType determinePaymentType()
        {
            PapaBobs.DTO.Enums.PaymentType paymentType;
            if (creditRadioButton.Checked)
            {
                paymentType = PapaBobs.DTO.Enums.PaymentType.Credit;
            }
            else 
            {
                paymentType = PapaBobs.DTO.Enums.PaymentType.Cash;
            }
            return paymentType;
        }

        private CrustType determineCrust()
        {
            PapaBobs.DTO.Enums.CrustType crust;
            if (!Enum.TryParse(crustDropDownList.SelectedValue, out crust))
            {
                throw new Exception("Please select a pizza crust.");
            }
            return crust;
        }

        private SizeType determineSize()
        {
            SizeType size;
            if (!Enum.TryParse(sizeDropDownList.SelectedValue, out size))
            {
                throw new Exception("Please select a pizza size.");
            }
            return size;
        }

        protected void recalculateTotalCost(object sender, EventArgs e)
        {
            if (sizeDropDownList.SelectedValue == string.Empty) return;
            if (crustDropDownList.SelectedValue == string.Empty) return;

            var order = buildOrder();
            try
            {
                TotalLabel.Text = PapaBobs.Domain.PizzaPriceManager.CalculateCost(order).ToString("C");
            }
            catch (Exception)
            {
                //swallow the error
            }
        }

        private PapaBobs.DTO.OrderDTO buildOrder()
        {
            var order = new PapaBobs.DTO.OrderDTO();
            order.Size = determineSize();
            order.Crust = determineCrust();
            order.Sausage = sausageCheckBox.Checked;
            order.Pepperoni = pepperoniCheckBox.Checked;
            order.Onion = onionsCheckBox.Checked;
            order.GreenPeppers = greenPeppersCheckBox.Checked;
            order.Name = nametextBox.Text;
            order.Address = addressTextBox.Text;
            order.Zip = zipTextBox.Text;
            order.Phone = phoneTextBox.Text;
            order.PaymentType = determinePaymentType();
            return order;
        }
    }
}