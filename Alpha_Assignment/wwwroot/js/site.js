document.addEventListener('DOMContentLoaded', () => {

    //Registration
   
    const forms = document.querySelectorAll('form');

    forms.forEach(form => {
        
        if (form.dataset.listenerAdded === 'true') return;

        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            clearErrorMessages(form)

            const formData = new FormData(form)

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal)
                        modal.style.display = 'none';

                    window.location.href = '/projects';
                }

                else if (res.status === 400) {
                    const data = await res.json();

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`);
                            if (input) {
                                input.classList.add('input-validation-error');
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`);
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error');
                            }
                        });
                    }
                }
            }
            catch {
                console.log('error submitting the form');
            }

        });

       
        form.dataset.listenerAdded = 'true';
    });




    // Open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]');
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) {
                modal.style.display = 'flex';
            }
        });
    });

    
    document.addEventListener("DOMContentLoaded", function () {
        document.querySelectorAll("button[data-modal='true']").forEach(button => {
            button.addEventListener("click", async function () {
                const projectCard = button.closest(".squeare");
                const projectId = projectCard.getAttribute("data-project-id");

                try {
                    const response = await fetch(`/Projects/editmodal/${projectId}`);

                    if (!response.ok) {
                        throw new Error("Kunde inte ladda editformuläret. Status: " + response.status);
                    }

                    const html = await response.text();

                   
                    const modalContainer = document.getElementById("editProjectModal");
                    modalContainer.innerHTML = html;

                  
                    const form = modalContainer.querySelector('form');
                    const projectData = JSON.parse(form.dataset.projectData); 

                    form.querySelector('[name="ProjectName"]').value = projectData.ProjectName;
                    form.querySelector('[name="ClientId"]').value = projectData.ClientId;
                    form.querySelector('[name="Description"]').value = projectData.Description;
                    form.querySelector('[name="StartDate"]').value = projectData.StartDate;
                    form.querySelector('[name="EndDate"]').value = projectData.EndDate;
                    form.querySelector('[name="Budget"]').value = projectData.Budget;
                    form.querySelector('[name="Status"]').value = projectData.Status;

                   
                    modalContainer.classList.add("show");
                    modalContainer.style.display = "block";

                    
                    modalContainer.querySelector("[data-close]").addEventListener("click", () => {
                        modalContainer.classList.remove("show");
                        modalContainer.style.display = "none";
                    });

                } catch (err) {
                    console.error("Fel vid hämtning av edit-modal:", err);
                }
            });
        });
    });

    









    ///* Get projects to edit*/
    //document.querySelectorAll('[data-modal="true"][data-target="#editProjectModal"]').forEach(button => {
    //    button.addEventListener('click', async () => {
    //        const projectId = button.id.replace('toggleBtn-', '');

    //        try {
    //            const response = await fetch(`/Projects/editprojectform/${projectId}`);
    //            const html = await response.text();
                
    //            document.querySelector('#editProjectModal .modal-content').innerHTML = html;
    //        } catch (error) {
    //            console.error('Error fetching project data:', error);
    //        }
    //    });
    //});











    // Close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]');
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');
            if (modal) {
                modal.style.display = 'none';
                modal.querySelectorAll('form').forEach(form => {
                    form.reset();
                });
            }
        });
    });

    document.querySelectorAll('.tab').forEach(tab => {
        tab.addEventListener('click', () => {
            const selectedStatus = tab.getAttribute('data-status').toLowerCase();

            
            document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

          
            document.querySelectorAll('[data-project-id]').forEach(project => {
                const status = project.getAttribute('data-status')?.toLowerCase();

                if (selectedStatus === "all" || status === selectedStatus) {
                    project.style.display = 'block';
                } else {
                    project.style.display = 'none';
                }
            });
        });
    });

   
    document.querySelector('.tab[data-status="ALL"]').click();




    // Dropdowns
    const dropdowns = document.querySelectorAll('[data-type="dropdown"]');
    document.addEventListener('click', function (event) {
        let clickedDropdown = null;

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);

            if (dropdown.contains(event.target)) {
                clickedDropdown = targetElement;

                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show');
                    }
                });

                if (!targetElement.classList.contains('dropdown-show')) {
                    targetElement.classList.add('dropdown-show');
                }

                return;
            }
        });

        if (!clickedDropdown && !event.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show');
            });
        }
    });


    


}); 

//Delete projects

async function deleteProject(projectId) {

    if (!confirm('Are you sure you want to delete this project?')) {
        return;
    }

    try {
        const response = await fetch(`/Projects/delete/${projectId}`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const data = await response.json();

        if (data.success) {
            window.location.reload();
        } else {
            alert('Error deleting project: ' + (data.error || 'Unknown error'));
        }
    } catch (error) {
        console.error('Error deleting project:', error);
        alert('An unexpected error occurred.');
    }

}



function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });
}





