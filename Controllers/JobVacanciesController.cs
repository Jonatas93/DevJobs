namespace DevJobs.API.Controllers
{
    using DevJobs.API.Entities;
    using DevJobs.API.Models;
    using DevJobs.API.Persistence;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/job-vacancies")]
    [ApiController]
    public class JobVacanciesController : ControllerBase
    {
        private readonly DevJobsContext _context;

        public JobVacanciesController(DevJobsContext context){
            _context = context;
        }
        
        [HttpGet]
        public IActionResult GetAll()
        {
            var jobVacancies =  _context.JobVacancies;
            return Ok(jobVacancies);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var jobVacancy = _context.JobVacancies
                    .Include(jv => jv.Applications)
                    .SingleOrDefault(x => x.Id == id);
            
            if(jobVacancy == null)
                return NotFound();

            return Ok(jobVacancy);
        } 

        [HttpPost]
        public IActionResult Post(AddJobVacancyInputModel model)
        {
            var jobVacancy = new JobVacancy(model.Title, model.Description, model.Company, model.IsRemote, model.SalaryRange);

            _context.JobVacancies.Add(jobVacancy);
            _context.SaveChanges();

            //Retorna o nome da action para fazer a consulta do item cadastrado e retornar o item cadastrado.
            return CreatedAtAction("GetById", new {id = jobVacancy.Id}, jobVacancy);
        } 

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateJobVacancyInputModel model)
        {
            var jobVacancy = _context.JobVacancies.SingleOrDefault(x => x.Id == id);
            
            if(jobVacancy == null)
                return NotFound();
            
            jobVacancy.Update(model.Title, model.Description);
            _context.SaveChanges();

            return NoContent();
        } 
    }
}