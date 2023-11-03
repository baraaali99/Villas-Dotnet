using System.Net;
using AutoMapper;
using firstDotnetProject.Repository.iRepository;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace firstDotnetProject.Controllers;


    [Route("api/VillaNumbers")]
    [ApiController]
public class VillaNumbersApiController : ControllerBase
{
    private readonly IVillaNumberRepository _villaNumberRepository;
    private readonly IMapper _mapper;

    public VillaNumbersApiController(IVillaNumberRepository villaNumberRepository, IMapper mapper)
    {
        _villaNumberRepository = villaNumberRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse>> GetVillasNumber()
    {
        var response = new ApiResponse();
        try
        {
            IEnumerable<VillaNumber> villaNumbers = await _villaNumberRepository.GetAllAsync();
            response.Result = _mapper.Map<VillaNumber>(villaNumbers);
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return response;
    }

    [HttpGet ("{id:int}")]
    public async Task<ActionResult<ApiResponse>> GetVillaNumber(int id)
    {
        ApiResponse response = new ApiResponse();
        try
        {
            if (id == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            VillaNumber villaNumber = await _villaNumberRepository.
                GetAsync(u => u.VillaNo == id);

            if (villaNumber == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }
            response.Result = _mapper.Map<VillaNumberDto>(villaNumber);
            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;

            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return response;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDto villaNumberDto)
    {
        ApiResponse response = new ApiResponse();
        try
        {
            if (villaNumberDto == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            if (await _villaNumberRepository.GetAsync(u => u.VillaNo == villaNumberDto.VillaNo) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            VillaNumber model = _mapper.Map<VillaNumber>(villaNumberDto);
            await _villaNumberRepository.CreateAsync(model);
            response.Result = _mapper.Map<VillaNumberDto>(model);
            response.StatusCode = HttpStatusCode.Created;

            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return response;
    }

    [HttpDelete ("{id:int}")]
    public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
    {
        ApiResponse response = new ApiResponse();

        try
        {
            var villaNumber = await _villaNumberRepository.GetAsync(u => u.VillaNo == id);

            if (villaNumber == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            await _villaNumberRepository.RemoveAsync(villaNumber);
            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;
            
            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return response;
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaNumberUpdateDto villaNumberUpdateDto)
    {
        ApiResponse response = new ApiResponse();

        try
        {
            if (villaNumberUpdateDto == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            VillaNumber model = await _villaNumberRepository.GetAsync(u => u.VillaNo == id);
            if (model == null)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(response);
            }

            villaNumberUpdateDto.VillaNo = id;
            model = _mapper.Map<VillaNumber>(villaNumberUpdateDto);
            await _villaNumberRepository.UpdateAsync(model);
            response.StatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
        catch (Exception e)
        {
            response.IsSuccess = false;
            response.ErrorMessages = new List<string>() { e.ToString() };
        }

        return response;
    }
}