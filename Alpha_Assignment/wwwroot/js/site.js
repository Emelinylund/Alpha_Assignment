document.addEventListener('DOMContentLoaded', () => {

    //open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]');
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);
            if (modal) {
                modal.style.display = 'flex';
            }
        })
    })
    //close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]')
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal')
            if (modal) {
                modal.style.display = 'none'

                modal.querySelectorAll('form').forEach(form => {
                    form.reset()
                })
            }
        })
    })
    //Registration
    const forms = document.querySelectorAll('form')
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault()

            
            clearErrorMessages(form)
            

            const formData = new FormData(form)
            
            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                })

                if (res.ok) {
                    const modal = form.closest('.modal')
                    if (modal)
                        modal.style.display = 'none';

                    window.location.href = '/projects';
                }

               else if (res.status === 400) {
                    const data = await res.json()


                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            const input = form.querySelector(`[name="${key}"]`)
                            if (input) {
                                input.classList.add('input-validation-error')
                            }

                            const span = form.querySelector(`[data-valmsg-for="${key}"]`)
                            if (span) {
                                span.innerText = data.errors[key].join('\n');
                                span.classList.add('field-validation-error')
                            }
                        })
                    }
                }
            }
            catch {
                console.log('error submitting the form')
            }

        })
    })



    const dropdowns = document.querySelectorAll('[data-type="dropdown"]')
    
    document.addEventListener('click', function (event)
    {
        let clickedDropdown = null

        dropdowns.forEach(dropdown => {
            const targetId = dropdown.getAttribute('data-target');
            const targetElement = document.querySelector(targetId);

          

            if (dropdown.contains(event.target)) {
                clickedDropdown = targetElement;

               

                document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                    if (openDropdown !== targetElement) {
                        openDropdown.classList.remove('dropdown-show');
                    }
                })


                if (!targetElement.classList.contains('dropdown-show')) {
                    targetElement.classList.add('dropdown-show');
                }

                return; 

            }
        })

        if (!clickedDropdown && !event.target.closest('.dropdown')) {
            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                openDropdown.classList.remove('dropdown-show');
            })
        } 
    })


    



})

function clearErrorMessages(form) {

    form.querySelectorAll('[data-val="true"]').forEach(input => {
        input.classList.remove('input-validation-error')
    })

    form.querySelectorAll('[data-valmsg-for]').forEach(span => {
        span.innerText = ''
        span.classList.remove('field-validation-error')
    })
}

function addErrorMessage(key, errorMessage) {
    
}


    // Ta bort projektet
    document.querySelectorAll('.delete-link').forEach(button => {
        button.addEventListener('click', async function (e) {
            e.preventDefault();
            const projectId = this.getAttribute('data-project-id');

            const confirmDelete = confirm('Are you sure you want to delete this project?');
            if (!confirmDelete) return;

            const response = await fetch(`/Projects/delete/${projectId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            });

            const data = await response.json();

            if (data.Success) {
                // Ta bort projektet från DOM:en
                const projectElement = document.querySelector(`[data-project-id="${projectId}"]`);
                if (projectElement) {
                    projectElement.remove();
                }
            } else {
                alert('Error deleting project: ' + data.Error);
            }
        });
    });



