public class WorkflowEngine : IWorkflowEngine
{
    private readonly AppDbContext _db;
    private readonly IEmailService _email;

    public WorkflowEngine(AppDbContext db, IEmailService email)
    {
        _db = db;
        _email = email;
    }

    public async Task InitializeWorkflowAsync(LeaveRequest request)
    {
        var definition = await _db.WorkflowDefinitions
            .Include(x => x.Steps)
            .FirstAsync(x => x.LeaveType == request.LeaveType);

        var firstStepNumber = definition.Steps.Min(s => s.StepNumber);
        var firstSteps = definition.Steps.Where(s => s.StepNumber == firstStepNumber).ToList();

        foreach (var step in firstSteps)
        {
            request.WorkflowInstances.Add(new WorkflowInstance
            {
                LeaveRequestId = request.Id,
                StepDefinitionId = step.Id,
                Status = WorkflowApprovalStatus.Pending
            });

            await _email.SendAsync("Approval Required", 
                $"A leave request requires your approval", 
                await ResolveApproverEmailAsync(step.ApproverRole));
        }

        request.Status = LeaveRequestStatus.InProgress;

        await _db.SaveChangesAsync();
    }

    public async Task ProcessApprovalAsync(Guid workflowInstanceId, string approverUserId, bool isApproved)
    {
        var instance = await _db.WorkflowInstances
            .Include(x => x.WorkflowsStepDefinition)
            .Include(x => x.LeaveRequest)
            .FirstAsync(x => x.Id == workflowInstanceId);

        if (isApproved)
        {
            instance.Status = WorkflowApprovalStatus.Approved;
            instance.ActionByUserId = approverUserId;
            instance.ActionDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            await ProcessNextSteps(instance);
        }
        else
        {
            instance.Status = WorkflowApprovalStatus.Rejected;
            instance.LeaveRequest.Status = LeaveRequestStatus.Rejected;

            await _db.SaveChangesAsync();

            // notify employee
            await _email.SendAsync("Leave Request Rejected",
                "Your leave request was rejected.",
                await GetEmployeeEmailAsync(instance.LeaveRequest.EmployeeId));
        }
    }

    private async Task ProcessNextSteps(WorkflowInstance instance)
    {
        var request = instance.LeaveRequest;

        var definition = await _db.WorkflowDefinitions
            .Include(x => x.Steps)
            .FirstAsync(x => x.LeaveType == request.LeaveType);

        var currentStep = definition.Steps.First(x => x.Id == instance.StepDefinitionId);

        // Parallel: wait until all in same step are approved
        if (currentStep.IsParallel)
        {
            bool allApproved = request.WorkflowInstances
                .Where(x => x.StepDefinitionId == currentStep.Id)
                .All(x => x.Status == WorkflowApprovalStatus.Approved);

            if (!allApproved)
                return; // still waiting for others
        }

        // Get next step(s)
        var nextStepNumber = currentStep.StepNumber + 1;
        var nextSteps = definition.Steps.Where(x => x.StepNumber == nextStepNumber).ToList();

        if (!nextSteps.Any())
        {
            request.Status = LeaveRequestStatus.Approved;
            await _db.SaveChangesAsync();

            await _email.SendAsync("Leave Request Approved",
                "Your leave request has been fully approved.",
                await GetEmployeeEmailAsync(request.EmployeeId));

            return;
        }

        foreach (var next in nextSteps)
        {
            if (!ConditionPasses(next, request))
                continue;

            request.WorkflowInstances.Add(new WorkflowInstance
            {
                LeaveRequestId = request.Id,
                StepDefinitionId = next.Id,
                Status = WorkflowApprovalStatus.Pending
            });

            await _email.SendAsync("Leave Approval Required",
                "A leave request requires your approval.",
                await ResolveApproverEmailAsync(next.ApproverRole));
        }

        await _db.SaveChangesAsync();
    }

    private bool ConditionPasses(WorkflowStepDefinition step, LeaveRequest req)
    {
        if (string.IsNullOrWhiteSpace(step.ConditionExpression))
            return true;

        // Simple rule engine — extend as needed
        if (step.ConditionExpression == "TotalDays > 3")
        {
            int total = req.DateTo.DayNumber - req.DateFrom.DayNumber;
            return total > 3;
        }

        return true;
    }

    private Task<string> ResolveApproverEmailAsync(string role)
    {
        // integrate with your Users table
        return Task.FromResult("manager@example.com");
    }

    private Task<string> GetEmployeeEmailAsync(Guid employeeId)
    {
        return Task.FromResult("employee@example.com");
    }
}