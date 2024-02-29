/*--****************************************************************************
 --* Project Name    : Nfs.WebAPI.DyanamoDb
 --* Reference       : Amazon.DynamoDBv2.DataModel
 --* Description     : Product model
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         20/06/24  CR-XXXXX Original
 --****************************************************************************/
using Amazon.DynamoDBv2.DataModel;

namespace Nfs.WebAPI.DynamoDb.Domain;

/// <summary>
/// Represents a product
/// </summary>
[DynamoDBTable("Products")]
public partial class Product
{
    /// <summary>
    /// Gets or sets identifier
    /// </summary>
    [DynamoDBHashKey("Id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the barcode
    /// </summary>
    [DynamoDBRangeKey("Barcode")]
    public string? Barcode { get; set; }

    /// <summary>
    /// Gets or sets the name
    /// </summary>
    [DynamoDBProperty("Name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description
    /// </summary>
    [DynamoDBProperty("Description")]
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the price
    /// </summary>
    [DynamoDBProperty("Price")]
    public decimal Price { get; set; }
}