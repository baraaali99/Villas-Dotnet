using System.Net;
using AutoMapper;
using firstDotnetProject.Repository.iRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace firstDotnetProject.Controllers;
   // [Route("api/[controller]")]
    [Route("api/VillaApi")]
    [ApiController]
public class VillaApiController : ControllerBase
{
    private readonly IVillaRepository _dbVilla;
    private readonly IMapper _mapper;
    public VillaApiController(IVillaRepository dbVilla, IMapper mapper)
    {
        _dbVilla = dbVilla;
        _mapper = mapper;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> GetVillas()
    {
        var _response = new ApiResponse();
        try
        {
            IEnumerable<Villa> villaList = await _dbVilla.GetAllAsync();
            _response.Result = _mapper.Map<List<VillaDto>>(villaList);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return  Ok(_response);
        }catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpGet("{id:int}", Name = "GetVilla")]
    [Authorize (Roles = "admin")]
    public async Task<ActionResult<ApiResponse>> GetVilla(int id)
    {
        var _response = new ApiResponse();
        try
        {
            if (id == 0)
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id );

            if (villa == null)
            {
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            _response.Result = _mapper.Map<VillaDto>(villa);
            _response.StatusCode = HttpStatusCode.OK;
            _response.IsSuccess = true;
            return Ok(_response);
        }catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpPost]
    [Authorize (Roles = "admin")]
    public async Task<ActionResult<ApiResponse>> CreateVilla([FromBody] VillaCreateDto villaCreateDto)
    {
        var _response = new ApiResponse();
        try
        {


            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == villaCreateDto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa already Exists!");
                return BadRequest(ModelState);
            }

            if (villaCreateDto == null)
            {
                return BadRequest(villaCreateDto);
            }

            Villa model = _mapper.Map<Villa>(villaCreateDto);
            await _dbVilla.CreateAsync(model);
            _response.Result = _mapper.Map<VillaDto>(model);
            _response.StatusCode = HttpStatusCode.Created;
            _response.IsSuccess = true;
            
            return Ok(_response);
        }catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpDelete("{id:int}" , Name = "DeleteVilla")]
    [Authorize(Roles = "CUSTOM")]
    public async Task<ActionResult<ApiResponse>> DeleteVilla(int id)
    {
        var _response = new ApiResponse();
        try
        {
            if (id == 0)
            {
                return BadRequest();
            }

            var villa = await _dbVilla.GetAsync(u => u.Id == id);

            if (villa == null)
            {
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _dbVilla.RemoveAsync(villa);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }

    [HttpPut("{id:int}", Name = "UpdateVilla")]
    [Authorize(Roles = "CUSTOM")]
    public async Task<ActionResult<ApiResponse>> UpdateVilla(int id, [FromBody] VillaUpdateDto villaUpdateDto)
    {
        var _response = new ApiResponse();
        try
        {
            // in case i am changing the name to a name that already exists!
            if (await _dbVilla.GetAsync(u => u.Name.ToLower() == villaUpdateDto.Name.ToLower()) != null)
            {
                _response.ErrorMessages = new List<string>() { "Villa name already Exists!" };
                _response.IsSuccess = false;
                _response.StatusCode = HttpStatusCode.BadRequest;
                //ModelState.AddModelError("CustomError", "Villa name already Exists!");
                return BadRequest(_response);
            }

            if (villaUpdateDto == null)
            {
                return BadRequest();
            }

            Villa model = await _dbVilla.GetAsync(u => u.Id == id , false);
            if (model == null)
            {
                return BadRequest();
            }

            villaUpdateDto.Id = id;
            model = _mapper.Map<Villa>(villaUpdateDto);

            await _dbVilla.UpdateAsync(model);
            _response.StatusCode = HttpStatusCode.NoContent;
            _response.IsSuccess = true;
            return Ok(_response);
        }catch (Exception e)
        {
            _response.IsSuccess = false;
            _response.ErrorMessages = new List<string>() { e.ToString() };
        }
        return _response;
    }
    
    [HttpPatch("{id:int}", Name = "UpdatePartialVilla")]
    public async Task<ActionResult<ApiResponse>> UpdatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
    {
        var _response = new ApiResponse();
        if (patchDto == null || id == 0)
            return BadRequest();
        
        var villa = await _dbVilla.GetAsync(u => u.Id == id , tracked:false);
        VillaUpdateDto villaUpdateDto = _mapper.Map<VillaUpdateDto>(villa);
        
        if (villa == null)
            return BadRequest();
        
        patchDto.ApplyTo(villaUpdateDto , ModelState);

        Villa model = _mapper.Map<Villa>(villaUpdateDto);
        await _dbVilla.UpdateAsync(model);
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        _response.StatusCode = HttpStatusCode.NoContent;
        _response.IsSuccess = true;
        return Ok(_response);
    }
}
