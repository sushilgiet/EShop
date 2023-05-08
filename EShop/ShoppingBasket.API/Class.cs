namespace ShoppingBasket.API
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    namespace YourNamespace
    {
        public class Order
        {
            public int Id { get; set; }
            public string CustomerName { get; set; }
            public List<OrderLineItem> LineItems { get; set; }

            public Order(string customerName, List<OrderLineItem> lineItems)
            {
                CustomerName = customerName;
                LineItems = lineItems;
            }

            public void Validate()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                if (results.Count > 0)
                {
                    var validationErrors = new List<string>();
                    foreach (var validationResult in results)
                    {
                        validationErrors.Add(validationResult.ErrorMessage);
                    }
                    throw new ValidationException(string.Join(",", validationErrors));
                }
            }
        }

        public class OrderLineItem
        {
            public int Id { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
            public Product Product { get; set; }

            public OrderLineItem(int quantity, decimal price, Product product)
            {
                Quantity = quantity;
                Price = price;
                Product = product;
            }

            public void Validate()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                if (results.Count > 0)
                {
                    var validationErrors = new List<string>();
                    foreach (var validationResult in results)
                    {
                        validationErrors.Add(validationResult.ErrorMessage);
                    }
                    throw new ValidationException(string.Join(",", validationErrors));
                }
            }
        }

        public class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }

            public Product(string name, decimal price)
            {
                Name = name;
                Price = price;
            }

            public void Validate()
            {
                var context = new ValidationContext(this, serviceProvider: null, items: null);
                var results = new List<ValidationResult>();
                Validator.TryValidateObject(this, context, results, validateAllProperties: true);

                if (results.Count > 0)
                {
                    var validationErrors = new List<string>();
                    foreach (var validationResult in results)
                    {
                        validationErrors.Add(validationResult.ErrorMessage);
                    }
                    throw new ValidationException(string.Join(",", validationErrors));
                }
            }
        }
    }

}
