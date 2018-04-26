using Acme.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acme.Biz
{
    /// <summary>
    /// Manages the vendors from whom we purchase our inventory.
    /// </summary>
    public class Vendor 
    {
        public enum IncludeAddress { Yes, No };
        public enum SendCopy { Yes, No};

        public int VendorId { get; set; }
        public string CompanyName { get; set; }
        public string Email { get; set; }

        /// <summary>
        /// Sends a product order to the vendor.
        /// </summary>
        /// <param name="product"></param>
        /// <param name="quantity"></param>
        /// <param name="deliverBy"></param>
        /// <param name="instructions"></param>
        /// <returns></returns>
        public OperationResult PlaceOrder(Product product, int quantity,
                                          DateTimeOffset? deliverBy = null,
                                          string instructions = "standard delivery")
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(nameof(quantity));

            var success = false;

            var orderText = "Order from Acme, Inc" + Environment.NewLine +
                            "Product: " + product.ProductCode +
                                            Environment.NewLine +
                            "Quantity: " + quantity;

            if (deliverBy.HasValue)
            {
                orderText += Environment.NewLine +
                             "Deliver By: " + deliverBy.Value.ToString("d");
            }

            if (!String.IsNullOrWhiteSpace(instructions))
            {
                orderText += Environment.NewLine +
                             "Instructions: " + instructions;
            }

            var emailService = new EmailService();
            var confirmation = emailService.SendMessage("New Order", orderText,
                                                                     this.Email);

            if (confirmation.StartsWith("Message sent:"))
            {
                success = true;
            }

            var operationResult = new OperationResult(success, orderText);
            return operationResult;
        }
        
        /// <summary>
        /// Sends a product order to the vendor.
        /// </summary>
        /// <param name="product">Product to order.</param>
        /// <param name="quantity">Quantity of the product to order.</param>
        /// <param name="includeAddress">True to incude shipping address.</param>
        /// <param name="sendCopy">True to send copy of email to current address.</param>
        /// <returns>Success flag and order text.</returns>
        public OperationResult PlaceOrder(Product product, int quantity,
                                          IncludeAddress includeAddress, SendCopy sendCopy)
        {
            var orderText = "Test";
            if (includeAddress == IncludeAddress.Yes) orderText += " With Address";
            if (sendCopy == SendCopy.Yes) orderText += " With Copy";

            var operationResult = new OperationResult(true, orderText);
            return operationResult;
        }

        /// <summary>
        /// Sends an email to welcome a new vendor.
        /// </summary>
        /// <returns></returns>
        public string SendWelcomeEmail(string message)
        {
            var emailService = new EmailService();
            var subject = "Hello " + this.CompanyName;
            var confirmation = emailService.SendMessage(subject,
                                                        message, 
                                                        this.Email);
            return confirmation;
        }

        public override string ToString()
        {
            string vendorInfo = "Vendor: " + this.CompanyName;
            return vendorInfo;
        }
    }
}
