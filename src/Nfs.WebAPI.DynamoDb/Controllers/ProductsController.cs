/*--****************************************************************************
 --* Project Name    : WebApi-DynamoDb-CRUD
 --* Reference       : Microsoft.AspNetCore.Mvc
 --*                   Amazon.DynamoDBv2.DataModel
 --*                   Nfs.WebAPI.DynamoDb.Domain
 --* Description     : Products controller
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/11/24  CR-XXXXX Original
 --****************************************************************************/
using Amazon.DynamoDBv2.DataModel;
using Microsoft.AspNetCore.Mvc;
using Nfs.WebAPI.DynamoDb.Domain;

namespace Nfs.WebAPI.DynamoDb.Controllers;

/// <summary>
/// Represents a products api controller
/// </summary>
[Route("api/v1/[controller]")]
public partial class ProductsController : BaseApiController
{
    #region Fields

    private readonly IDynamoDBContext _dynamoDbContext;

    #endregion

    #region Ctor

    public ProductsController(IDynamoDBContext dynamoDbContext)
    {
        _dynamoDbContext = dynamoDbContext;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Gets list of product items
    /// </summary>
    /// <returns>List of products</returns>
    /// GET: api/v1/Products/List
    [HttpGet]
    [Route("List")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> ListAsync()
    {
        //try to get products
        var products = await _dynamoDbContext.ScanAsync<Product>(default).GetRemainingAsync();

        //prepare the model

        return Ok(products);
    }

    /// <summary>
    /// Gets a product
    /// </summary>
    /// <param name="id">Product identifier</param>
    /// <param name="barcode">Barcode</param>
    /// <returns>Product</returns>
    /// GET: api/v1/Products/GetById/1/123
    [HttpGet]
    [Route("GetById/{id}/{barcode}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> GetProductByIdAsync([FromRoute] string id, [FromRoute] string barcode)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(barcode))
            return BadRequest();

        //try to get product with the specified id and barcode
        var product = await _dynamoDbContext.LoadAsync<Product>(id, barcode);
        if (product == null)
            return NotFound("No product found with this specified id.");

        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="product">Product dto model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    /// POST: api/v1/Products/Create
    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> CreateAsync([FromBody] Product model)
    {
        if (model == null)
            return BadRequest();

        if (ModelState.IsValid)
        {
            //check if product already exists
            var productz = await _dynamoDbContext.LoadAsync<Product>(model.Id, model.Barcode);

            if (productz != null)
                return BadRequest($"Product with Id {model.Id} and Barcode {model.Barcode} already exists.");

            //insert product
            await _dynamoDbContext.SaveAsync(model);
        }

        return Ok(model);
    }

    /// <summary>
    /// Update product
    /// </summary>
    /// <param name="product">Product dto model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    /// PUT: api/v1/Products/Update
    [HttpPut]
    [Route("Update")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> UpdateAsync([FromBody] Product model)
    {
        if (model == null)
            return BadRequest();

        if (ModelState.IsValid)
        {
            //check if product already exists
            var productz = await _dynamoDbContext.LoadAsync<Product>(model.Id, model.Barcode);

            if (productz == null) return NotFound("No product found with the specified id and barcode.");

            await _dynamoDbContext.SaveAsync(model);
        }

        return Ok(model);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product identifier</param>
    /// <param name="barcode">Barcode</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    [HttpDelete]
    [Route("Delete/{id}/{barcode}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> DeleteAsync([FromRoute] string id, [FromRoute] string barcode)
    {
        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(barcode))
            return BadRequest();

        //Check if the product already exists
        var product = await _dynamoDbContext.LoadAsync<Product>(id, barcode);

        if (product == null) return NotFound();

        await _dynamoDbContext.DeleteAsync(product);

        //return empty content response (204)
        return NoContent();
    }

    #endregion
}