

    // Wait for the DOM to load
	document.addEventListener("DOMContentLoaded", function() {
        // Get current URL path
        const currentUrl = window.location.pathname;

	// Select all sidebar links
	const menuItems = document.querySelectorAll('.side-menu a');

        menuItems.forEach(item => {
            // Check if the link's href matches the current URL
            if (item.getAttribute('href') === currentUrl) {
		// Remove active class from any previously active item
		document.querySelector('.side-menu .active')?.classList.remove('active');

	// Add active class to the matching item
	item.parentElement.classList.add('active');
            }
        });
    });


// Handle sidebar toggle after DOM is fully loaded

    const menuBar = document.querySelector('#content nav .bx.bx-menu');
    const sidebar = document.getElementById('sidebar');
    const brandText = document.querySelector('#sidebar .brand span');
  
    menuBar.addEventListener('click', () => {
        const isHidden = sidebar.classList.toggle('hide');
        brandText.classList.toggle('hide');
      
    });



const searchButton = document.querySelector('#content nav form .form-input button');
const searchButtonIcon = document.querySelector('#content nav form .form-input button .bx');
const searchForm = document.querySelector('#content nav form');

searchButton.addEventListener('click', function (e) {
	if (window.innerWidth < 576) {
		e.preventDefault();
		searchForm.classList.toggle('show');
		if (searchForm.classList.contains('show')) {
			searchButtonIcon.classList.replace('bx-search', 'bx-x');
		} else {
			searchButtonIcon.classList.replace('bx-x', 'bx-search');
		}
	}
})





if (window.innerWidth < 768) {
	sidebar.classList.add('hide');
} else if (window.innerWidth > 576) {
	searchButtonIcon.classList.replace('bx-x', 'bx-search');
	searchForm.classList.remove('show');
}


window.addEventListener('resize', function () {
	if (this.innerWidth > 576) {
		searchButtonIcon.classList.replace('bx-x', 'bx-search');
		searchForm.classList.remove('show');
	}
})



const switchMode = document.getElementById('switch-mode');

switchMode.addEventListener('change', function () {
	if (this.checked) {
		document.body.classList.add('dark');
	} else {
		document.body.classList.remove('dark');
	}
})