// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
/**
* Template Name: Anyar - v4.7.1
* Template URL: https://bootstrapmade.com/anyar-free-multipurpose-one-page-bootstrap-theme/
* Author: BootstrapMade.com
* License: https://bootstrapmade.com/license/
*/
$(document).ready(function () {

    $('#tester').DataTable({
       
        "serverSide": true,
        "processing": true,
        "searching": { regex: true },
        // Ajax Filter
        "ajax": {
            url: "/Home/AjaxHandler",
            type: "POST",
            contentType: "application/json",
            dataType: "json",
            data: function (d) {
                return JSON.stringify(d);
            }
        },
        "columns": [
            { "data": "crasH_ID" },
            { "data": "crasH_DATETIME" },
            { "data": "crasH_SEVERITY_ID" },
            { "data": "route" },
            { "data": "milepoint" },
            { "data": "laT_UTM_Y" },
            { "data": "lonG_UTM_X" },
            { "data": "maiN_ROAD_NAME" },
            { "data": "city" },
            { "data": "countY_NAME" },
            { "data": "pedestriaN_INVOLVED" },
            { "data": "worK_ZONE_RELATED" },
            { "data": "bicyclisT_INVOLVED" },
            { "data": "motorcyclE_INVOLVED" },
            { "data": "impropeR_RESTRAINT" },
            { "data": "unrestrained" },
            
            { "data": "dui" },
            { "data": "intersectioN_RELATED" },
            { "data": "wilD_ANIMAL_RELATED" },
            { "data": "domestiC_ANIMAL_RELATED" },
            { "data": "overturN_ROLLOVER" },
            { "data": "commerciaL_MOTOR_VEH_INVOLVED" },
      
            { "data": "distracteD_DRIVING" },
            { "data": "drowsY_DRIVING" },
            { "data": "nighT_DARK_CONDITION" },
            { "data": "oldeR_DRIVER_INVOLVED" },
            {"data" : "roadwaY_DEPARTURE"},          
            { "data": "singlE_VEHICLE" },
            { "data": "teenagE_DRIVER_INVOLVED" },
                    
                  
            
           
        ],

    });

    
});

//CRASH_ID</td >
//CRASH_DATETIME</td >
//ROUTE</td >
//MILEPOINT</td >
//LATITUDE</td >
//LONGITUDE</td >
//MAIN_ROAD_NAME</td >
//CITY</td >
//COUNTY_NAME</td >
//PEDESTRIAN_INVOLVED</td >
//WORK_ZONE_RELATED</td >
//BICYCLIST_INVOLVED</td >
//MOTORCYCLE_INVOLVED</td >
//IMPROPER_RESTRAINT</td >
//UNRESTRAINED</td >
//CRASH_SEVERITY_ID</td >
//DUI</td >
//INTERSECTION_RELATED</td >
//WILD_ANIMAL_RELATED</td >
//DOMESTIC_ANIMAL_RELATED</td >
//OVERTURN_ROLLOVER</td >
//COMMERCIAL_MOTOR_VEH_INVOLVED

//$(document).ready(function () {
//    $('#mytable').DataTable({
//        "retrieve": true,
//        "scrollY": "450px",
//        "scrollCollapse": true,
//        "paging": true,
//        "scrollX": true

//    });


//});

// Actually code we are using

//This method confrims whether you want the accident to be deleted or not
function ConfirmDelete(crashid) {



  

    if (confirm('Are you sure you want to save this thing into the database?')) {
        // Save it!

        //sends ajax request to delete if confrimed is true
        $.ajax({
            url: "/Home/Delete",
            data: { "crashid": crashid},
            type: 'POST'
        })

        location.reload(true);
        alert("Delete successful.")

    } else {
        // Do nothing!
        alert("Delete Cancelled");
    }
}


$(document).ready(function () {
    // Setup - add a text input to each footer cell
    $('#mytable tfoot th').each(function () {
        var title = $(this).text();
        $(this).html('<input  type="text" class="align-content-center" placeholder="filter by ' + title.toLowerCase() + '" />');
    });

    // DataTable
    var table = $('#mytable').DataTable({
        "destroy": true,
        "scrollY": "450px",
        "scrollX": "450px",
        "scrollCollapse": true,
        "paging": true,

        "scrollX": true,

        initComplete: function () {
            // Apply the search
            this.api().columns().every(function () {
                var that = this;

                $('input', this.footer()).on('keyup change clear', function () {
                    if (that.search() !== this.value) {
                        that
                            .search(this.value)
                            .draw();
                    }
                });
            });
        }
    });


});



(function () {
    "use strict";

    /**
        * Easy selector helper function
        */
    const select = (el, all = false) => {
        el = el.trim()
        if (all) {
            return [...document.querySelectorAll(el)]
        } else {
            return document.querySelector(el)
        }
    }

    /**
        * Easy event listener function
        */
    const on = (type, el, listener, all = false) => {
        let selectEl = select(el, all)
        if (selectEl) {
            if (all) {
                selectEl.forEach(e => e.addEventListener(type, listener))
            } else {
                selectEl.addEventListener(type, listener)
            }
        }
    }

    /**
        * Easy on scroll event listener 
        */
    const onscroll = (el, listener) => {
        el.addEventListener('scroll', listener)
    }

    /**
        * Navbar links active state on scroll
        */
    let navbarlinks = select('#navbar .scrollto', true)
    const navbarlinksActive = () => {
        let position = window.scrollY + 200
        navbarlinks.forEach(navbarlink => {
            if (!navbarlink.hash) return
            let section = select(navbarlink.hash)
            if (!section) return
            if (position >= section.offsetTop && position <= (section.offsetTop + section.offsetHeight)) {
                navbarlink.classList.add('active')
            } else {
                navbarlink.classList.remove('active')
            }
        })
    }
    window.addEventListener('load', navbarlinksActive)
    onscroll(document, navbarlinksActive)

    /**
        * Scrolls to an element with header offset
        */
    const scrollto = (el) => {
        let header = select('#header')
        let offset = header.offsetHeight

        if (!header.classList.contains('fixed-top')) {
            offset += 70
        }

        let elementPos = select(el).offsetTop
        window.scrollTo({
            top: elementPos - offset,
            behavior: 'smooth'
        })
    }

    /**
        * Header fixed top on scroll
        */
    let selectHeader = select('#header')
    let selectTopbar = select('#topbar')
    if (selectHeader) {
        const headerScrolled = () => {
            if (window.scrollY > 100) {
                selectHeader.classList.add('header-scrolled')
                if (selectTopbar) {
                    selectTopbar.classList.add('topbar-scrolled')
                }
            } else {
                selectHeader.classList.remove('header-scrolled')
                if (selectTopbar) {
                    selectTopbar.classList.remove('topbar-scrolled')
                }
            }
        }
        window.addEventListener('load', headerScrolled)
        onscroll(document, headerScrolled)
    }

    /**
        * Back to top button
        */
    let backtotop = select('.back-to-top')
    if (backtotop) {
        const toggleBacktotop = () => {
            if (window.scrollY > 100) {
                backtotop.classList.add('active')
            } else {
                backtotop.classList.remove('active')
            }
        }
        window.addEventListener('load', toggleBacktotop)
        onscroll(document, toggleBacktotop)
    }

    /**
        * Mobile nav toggle
        */
    on('click', '.mobile-nav-toggle', function (e) {
        select('#navbar').classList.toggle('navbar-mobile')
        this.classList.toggle('bi-list')
        this.classList.toggle('bi-x')
    })

    /**
        * Mobile nav dropdowns activate
        */
    on('click', '.navbar .dropdown > a', function (e) {
        if (select('#navbar').classList.contains('navbar-mobile')) {
            e.preventDefault()
            this.nextElementSibling.classList.toggle('dropdown-active')
        }
    }, true)

    /**
        * Scrool with ofset on links with a class name .scrollto
        */
    on('click', '.scrollto', function (e) {
        if (select(this.hash)) {
            e.preventDefault()

            let navbar = select('#navbar')
            if (navbar.classList.contains('navbar-mobile')) {
                navbar.classList.remove('navbar-mobile')
                let navbarToggle = select('.mobile-nav-toggle')
                navbarToggle.classList.toggle('bi-list')
                navbarToggle.classList.toggle('bi-x')
            }
            scrollto(this.hash)
        }
    }, true)

    /**
        * Scroll with ofset on page load with hash links in the url
        */
    window.addEventListener('load', () => {
        if (window.location.hash) {
            if (select(window.location.hash)) {
                scrollto(window.location.hash)
            }
        }
    });

    /**
        * Preloader
        */
    let preloader = select('#preloader');
    if (preloader) {
        window.addEventListener('load', () => {
            preloader.remove()
        });
    }

    /**
        * Clients Slider
        */
    new Swiper('.clients-slider', {
        speed: 400,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        slidesPerView: 'auto',
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        },
        breakpoints: {
            320: {
                slidesPerView: 2,
                spaceBetween: 40
            },
            480: {
                slidesPerView: 3,
                spaceBetween: 60
            },
            640: {
                slidesPerView: 4,
                spaceBetween: 80
            },
            992: {
                slidesPerView: 6,
                spaceBetween: 120
            }
        }
    });

    /**
        * Porfolio isotope and filter
        */
    window.addEventListener('load', () => {
        let portfolioContainer = select('.portfolio-container');
        if (portfolioContainer) {
            let portfolioIsotope = new Isotope(portfolioContainer, {
                itemSelector: '.portfolio-item',
                layoutMode: 'fitRows'
            });

            let portfolioFilters = select('#portfolio-flters li', true);

            on('click', '#portfolio-flters li', function (e) {
                e.preventDefault();
                portfolioFilters.forEach(function (el) {
                    el.classList.remove('filter-active');
                });
                this.classList.add('filter-active');

                portfolioIsotope.arrange({
                    filter: this.getAttribute('data-filter')
                });
                portfolioIsotope.on('arrangeComplete', function () {
                    AOS.refresh()
                });
            }, true);
        }

    });

    /**
        * Initiate portfolio lightbox 
        */
    const portfolioLightbox = GLightbox({
        selector: '.portfolio-lightbox'
    });

    /**
        * Initiate glightbox 
        */
    const gLightbox = GLightbox({
        selector: '.glightbox'
    });

    /**
        * Portfolio details slider
        */
    new Swiper('.portfolio-details-slider', {
        speed: 400,
        loop: true,
        autoplay: {
            delay: 5000,
            disableOnInteraction: false
        },
        pagination: {
            el: '.swiper-pagination',
            type: 'bullets',
            clickable: true
        }
    });

    /**
        * Animation on scroll
        */
    window.addEventListener('load', () => {
        AOS.init({
            duration: 1000,
            easing: 'ease-in-out',
            once: true,
            mirror: false
        })
    });

})()