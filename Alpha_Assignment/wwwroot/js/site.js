document.addEventListener('DOMContentLoaded', () => {

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

    document.querySelectorAll('[data-modal="true"][data-target="#editProjectModal"]').forEach(button => {
        button.addEventListener('click', async () => {
            const projectId = button.id.replace('toggleBtn-', '');

            try {
                const response = await fetch(`/Projects/get/${projectId}`);
                const data = await response.json();

                if (data) {
                    const form = document.querySelector('#editProjectModal form');
                    form.querySelector('input[name="Id"]').value = data.id;
                    form.querySelector('input[name="ProjectName"]').value = data.projectName;
                    form.querySelector('textarea[name="Description"]').value = data.description || '';
                    form.querySelector('input[name="StartDate"]').value = data.startDate;
                    form.querySelector('input[name="EndDate"]').value = data.endDate || '';
                    form.querySelector('select[name="Status"]').value = data.status;
                    form.querySelector('select[name="ClientId"]').value = data.clientId;
                }
            } catch (error) {
                console.error('Error fetching project data:', error);
            }
        });
    });

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
            const selectedStatus = tab.getAttribute('data-status');

            // Byt aktiv klass
            document.querySelectorAll('.tab').forEach(t => t.classList.remove('active'));
            tab.classList.add('active');

            // Filtrera projekt
            document.querySelectorAll('[data-project-id]').forEach(project => {
                const status = project.getAttribute('data-status');

                if (selectedStatus === "ALL" || status === selectedStatus) {
                    project.style.display = 'block';
                } else {
                    project.style.display = 'none';
                }
            });
        });
    });



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


    // Ta bort projektet
    document.querySelectorAll('.delete-link').forEach(button => {
        button.addEventListener('click', async function () {
            const projectId = this.getAttribute('id').replace('deleteProject-', '');

            const confirmDelete = confirm('Are you sure you want to delete this project?');
            if (!confirmDelete) return;

            try {
                const response = await fetch(`/Projects/delete/${projectId}`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                const data = await response.json();

                if (response.ok && data.Success) {
                    console.log(1)
                }

                if (data.Success) {

                    window.location.reload();
                    return;
                    
                } else {
                    alert('Error deleting project: ' + (data.Error || 'Unknown error'));
                    return;
                }
            } catch (error) {
                console.error('Error deleting project:', error);
                alert('An unexpected error occurred.');
            }
        });
    });


}); 





// Hjälp
function clearErrorMessages(form) {
    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error');
    });

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = '';
        span.classList.remove('field-validation-error');
    });
}





