using Microsoft.AspNetCore.Mvc;
using TARge25shop.Core.Dto;
using TARge25shop.Core.ServiceInterface;
using TARge25shop.Models.Kindergarten;

namespace TARge25shop.Controllers;

public class KindergartenController : Controller
{
    private readonly IKindergartenServices _service;
    public KindergartenController(IKindergartenServices service) => _service = service;

    public async Task<IActionResult> Index() => View(await _service.GetAllAsync());

    public async Task<IActionResult> Details(Guid id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpGet]
    public IActionResult Create() => View("CreateUpdate", new KindergartenFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(KindergartenFormViewModel vm)
    {
        if (!ModelState.IsValid) return View("CreateUpdate", vm);
        await _service.CreateAsync(ToDto(vm));
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Update(Guid id)
    {
        var item = await _service.GetAsync(id);
        if (item is null) return NotFound();
        return View("CreateUpdate", new KindergartenFormViewModel
        {
            Id = item.Id, GroupName = item.GroupName, ChildrenCount = item.ChildrenCount,
            KindergartenName = item.KindergartenName, TeacherName = item.TeacherName
        });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(Guid id, KindergartenFormViewModel vm)
    {
        if (vm.Id != id) return BadRequest();
        if (!ModelState.IsValid) return View("CreateUpdate", vm);
        if (!await _service.UpdateAsync(id, ToDto(vm))) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        var item = await _service.GetAsync(id);
        return item is null ? NotFound() : View(item);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        if (!await _service.DeleteAsync(id)) return NotFound();
        return RedirectToAction(nameof(Index));
    }

    private static KindergartenDto ToDto(KindergartenFormViewModel vm) => new()
    {
        GroupName = vm.GroupName, ChildrenCount = vm.ChildrenCount,
        KindergartenName = vm.KindergartenName, TeacherName = vm.TeacherName
    };
}
