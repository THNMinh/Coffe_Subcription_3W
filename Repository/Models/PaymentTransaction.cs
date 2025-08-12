using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class PaymentTransaction
{
    public int TransactionId { get; set; }

    public int UserId { get; set; }

    public decimal Amount { get; set; }

    public string Currency { get; set; } = null!;

    public string OrderId { get; set; } = null!;

    public string? TransactionNo { get; set; }

    public string? PaymentTime { get; set; }

    public string? TransactionStatus { get; set; }

    public string? BankCode { get; set; }

    public string? CardType { get; set; }

    public DateTime CreatedAt { get; set; }

    public string? IpAddress { get; set; }

    public virtual User User { get; set; } = null!;
}
