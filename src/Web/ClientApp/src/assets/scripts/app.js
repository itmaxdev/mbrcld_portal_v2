'use strict'
import 'swiper/scss'
import 'swiper/scss/grid'
import 'swiper/scss/pagination'
import '@fancyapps/ui/dist/fancybox/fancybox.css'
import 'odometer/themes/odometer-theme-default.css'
import '@scss/app.scss'

import SlimSelect from 'slim-select'
import inView from 'in-view'
import Lenis from 'lenis'
export const BODY = document.body
export const HTML = document.querySelector('html')
import tippy from 'tippy.js'
import Odometer from 'odometer'

import Swiper from 'swiper'

import { Autoplay, EffectCreative, Scrollbar, Grid } from 'swiper/modules'

import { Datepicker } from 'vanillajs-datepicker'

window.DatepickerInit = Datepicker
// // window.DateRangePickerInit = DateRangePicker;

// document.querySelectorAll("[data-datepicker]").forEach((elem) => {
//   const datepicker = new Datepicker(elem, {
//     autohide: true,
//     format: "dd/mm/yyyy",
//   });
// });
import { Fancybox } from '@fancyapps/ui'

Fancybox.bind('[data-fancyboxModal]', {
  dragToClose: false,
  closeButton: false,
  on: {
    done: (fancybox, slide) => {
      document.querySelector('.fancybox__content').setAttribute('data-lenis-prevent-wheel', 'true')
    },
  },
})
Fancybox.bind('[data-fancybox]', {
  on: {
    done: (fancybox, slide) => {
      document.querySelector('.fancybox__content').setAttribute('data-lenis-prevent-wheel', 'true')
    },
  },
})
window.Fancybox = Fancybox
window.SlimSelect = []
window.SlimSelectInit = SlimSelect
document.querySelectorAll('[data-select]').forEach((elem) => {
  let placeholderText = ''
  if (elem.dataset.placeholdertext) {
    placeholderText = elem.dataset.placeholdertext
  }

  let slimS = new SlimSelect({
    select: elem,
    settings: {
      showSearch: true,
      searchHighlight: true,
      placeholderText: placeholderText,
    },
  })
  elem.parentElement.classList.add('active')
  if (slimS.render.content.main) {
    slimS.render.content.main
      .querySelector('.ss-list')
      .setAttribute('data-lenis-prevent-wheel', 'true')
  }
  window.SlimSelect.push(slimS)
})

const lang = document.documentElement.getAttribute('lang')

window.homeSlider = () => {
  let homeSlider = new Swiper('[data-homeslider] .swiper', {
    modules: [Autoplay, EffectCreative],

    speed: 1000,
    slidesPerView: 1,
    // loop: true,
    autoplay: {
      delay: 4000,
      disableOnInteraction: false,
    },
    effect: 'creative',
    creativeEffect: {
      limitProgress: 1,

      prev: {
        shadow: false,
        translate: ['0%', '-100%', -400],
        origin: 'left bottom',
      },
      next: {
        shadow: false,
        translate: ['115%', '15%', -400],
        origin: 'left bottom',
      },
    },

    on: {},
  })
  return homeSlider
}
window.homeSlider()

export const webLoaded = () => {
  window.addEventListener('DOMContentLoaded', (event) => {
    //Burger menu
    burggerMenu()

    //body scroll event
    srollDirection()

    //viewport checker
    viewPortChecker()

    //smooth scroll
    smoothScroll()
    // scrollProgress();

    //input field
    inputControls()

    // Sub menu click
    // subMenu();

    //Tab
    // tab();

    // body class
    detectOSAndApplyClass()
    detectBrowserClass()

    // Alert box
    alertClick()

    //file uploader
    // dropZone();

    //tooltip
    tooltip()

    //Odometer number animation
    odometer()

    // program tab progress calc
    setTimeout(() => {
      programTab()
    }, 300)

    //body calc
    bodyProperty()

    // return callback();
  })
}

export const odometer = () => {
  const createOdometer = (el, value, isFloat = false) => {
    const odometer = new Odometer({
      el: el,
      value: 0,
      format: isFloat ? '(dd).dd' : 'dd',
    })

    const options = {
      threshold: [0, 0.9],
    }

    const callback = (entries, observer) => {
      entries.forEach((entry) => {
        if (entry.isIntersecting) {
          odometer.update(value)
        } else {
          odometer.update(0)
        }
      })
    }

    const observer = new IntersectionObserver(callback, options)
    observer.observe(el)
  }

  let animcount = document.querySelectorAll('[data-animcount]')

  animcount.forEach((el) => {
    let targetValue = Number(el.dataset.animcount)

    createOdometer(el.querySelector('i'), targetValue, !Number.isInteger(targetValue))
  })
}
export const tooltip = () => {
  // avatar dropdown
  let avatardrops = document.querySelectorAll('[data-avatardrop]')

  avatardrops.forEach((el) => {
    let targetDrop = document.getElementById(el.dataset.avatardrop)

    if (targetDrop) {
      tippy(el, {
        content: targetDrop.innerHTML,
        allowHTML: true,

        interactive: true,
        appendTo: () => document.body,
        trigger: ' click', //mouseenter
      })
    }
  })
}

export const programTab = () => {
  const programProgressTab = document.querySelector('[data-ptab]')
  const activeLink = programProgressTab?.querySelector('.pLink.active')
  if (activeLink) {
    const activeListItem = activeLink.closest('li')
    const progressDiv = programProgressTab.querySelector('.progress')
    const listItemRect = activeListItem.getBoundingClientRect()
    const containerRect = programProgressTab.getBoundingClientRect()
    const relativeLeft = listItemRect.left - containerRect.left
    const listItemWidth = listItemRect.width
    const centerOffset = relativeLeft + listItemWidth / 2
    const offsetValue = Math.round(centerOffset)
    progressDiv.style.setProperty('--percentage', `${offsetValue}px`)
    progressDiv.classList.add('animate')

    const activeItemLeft = activeListItem.offsetLeft
    const activeItemWidth = activeListItem.offsetWidth
    const containerWidth = programProgressTab.offsetWidth
    const scrollCenterPosition = activeItemLeft - containerWidth / 2 + activeItemWidth / 2

    programProgressTab.scrollTo({
      left: scrollCenterPosition,
      behavior: 'smooth',
    })
  }
}

// export const dropZone = () => {
//   const dropZone = document.getElementById("drop-zone");
//   const fileInput = document.getElementById("file-input");
//   const selectBtn = document.getElementById("select-btn");
//   const previewArea = document.getElementById("preview-area");

//   // Prevent default drag behaviors
//   ["dragenter", "dragover", "dragleave", "drop"].forEach((eventName) => {
//     dropZone.addEventListener(eventName, preventDefaults, false);
//   });

//   function preventDefaults(e) {
//     e.preventDefault();
//     e.stopPropagation();
//   }

//   // Highlight drop zone when dragging over
//   ["dragenter", "dragover"].forEach((eventName) => {
//     dropZone.addEventListener(
//       eventName,
//       () => dropZone.classList.add("highlight"),
//       false
//     );
//   });

//   ["dragleave", "drop"].forEach((eventName) => {
//     dropZone.addEventListener(
//       eventName,
//       () => dropZone.classList.remove("highlight"),
//       false
//     );
//   });

//   // Handle dropped files
//   dropZone.addEventListener("drop", handleDrop, false);

//   function handleDrop(e) {
//     const dt = e.dataTransfer;
//     const files = dt.files;
//     handleFiles(files);
//   }

//   // Handle file selection via input
//   selectBtn.addEventListener("click", () => fileInput.click());
//   fileInput.addEventListener("change", (e) => handleFiles(e.target.files));

//   function handleFiles(files) {
//     for (const file of files) {
//       if (file.type.startsWith("image/")) {
//         const reader = new FileReader();
//         reader.onload = (event) => {
//           const img = document.createElement("img");
//           img.src = event.target.result;
//           previewArea.appendChild(img);
//         };
//         reader.readAsDataURL(file);
//       } else {
//         console.warn("Non-image file dropped:", file.name);
//       }
//     }
//   }
// };

export const subMenu = () => {
  let btns = document.querySelectorAll('[data-submenu]')

  btns.forEach((btn) => {
    btn.addEventListener('click', (e) => {
      e.preventDefault()
      if (btn.parentNode.classList.contains('active')) {
        btn.parentNode.classList.remove('active')
      } else {
        btns.forEach((b) => {
          b.parentNode.classList.remove('active')
        })
        setTimeout(() => {
          btn.parentNode.classList.add('active')
        }, 10)
      }
    })
  })
}

export const smoothScroll = () => {
  window.lenis = new Lenis({
    virtualScroll: ({ event }) => {
      const isOverrideKeyHeld = event.ctrlKey || event.metaKey || event.altKey
      if (isOverrideKeyHeld) {
        return false // Disable smooth scroll
      }
      return true // Keep smooth scroll enabled
    },
  })

  // window.lenis.on('scroll', (e) => {
  //   console.log(e)
  // })
  if (window.location.hash) {
    let target = document.querySelector(window.location.hash)

    if (target) {
      window.lenis.scrollTo(target, {
        lock: true,
        onComplete: () => {
          document.body.classList.remove('scrolling_top')
        },
      })
    }
  }
  // scroll click
  let menuLinks = document.querySelectorAll('[data-scrol]')

  menuLinks.forEach((link) => {
    link.addEventListener('click', (e) => {
      let target = link.dataset.scrol,
        el = document.getElementById(target)

      if (el) {
        e.preventDefault()
        const NavItems = document.querySelectorAll('.link')
        NavItems.forEach((element) => {
          element.classList.remove('active')

          //check duplicate nav
          if (element.dataset.scrol === target) {
            element.classList.add('active')
          }
        })
        link.classList.add('active')
        window.lenis.scrollTo(el, {
          lock: true,
          onComplete: () => {
            document.body.classList.remove('scrolling_top')
          },
        })

        // burger menu close
        document.querySelector('body').classList.remove('menuActive')
      }
    })
  })

  function raf(time) {
    window.lenis.raf(time)
    requestAnimationFrame(raf)
  }

  requestAnimationFrame(raf)
}

export const tab = () => {
  let tabList = document.querySelectorAll('[data-tab]'),
    tabContentS = document.querySelectorAll('.tabContent')

  tabList.forEach((tab, i) => {
    tab.addEventListener('click', (e) => {
      e.preventDefault()
      let targetID = tab.dataset.tab,
        targetEL = document.getElementById(targetID)

      if (targetEL) {
        //remove old active

        tabList.forEach((t) => {
          t.classList.remove('active')
        })

        tabContentS.forEach((t) => {
          t.classList.remove('active')
        })

        // document.querySelectorAll(".visionBox .iconBox img").forEach((t) => {
        //   t.classList.remove("active");
        // });

        //active
        tab.classList.add('active')
        //  document.getElementById(`icon${i}`).classList.add("active");
        // setTimeout(() => {
        targetEL.classList.add('active')
        //  }, 10);
      }
    })

    if (i == 0) {
      tab.click()
    }
  })
}

export const viewPortChecker = () => {
  setTimeout(() => {
    inView.offset(100)
    inView('[data-inView]')
      .on('enter', (el) => {
        el.classList.add('inView')
      })
      .on('exit', (el) => {
        //  el.classList.remove('inView')
      })
  }, 500)
}

export const burggerMenu = () => {
  let burgerButto = document.querySelector('[data-burger-menu]')
  if (burgerButto) {
    burgerButto.addEventListener('click', (e) => {
      e.preventDefault()
      // let target_ = burgerButto.dataset.burgerMenu

      if (BODY.classList.contains('menuActive')) {
        BODY.classList.remove('menuActive')
        burgerButto.classList.remove('active')
      } else {
        BODY.classList.add('menuActive')
        burgerButto.classList.add('active')
      }
    })
  }
}

export const srollDirection = () => {
  let tempScrollPos = 0
  document.addEventListener('scroll', () => {
    let currentScrollY = window.scrollY

    currentScrollY > 10 ? BODY.classList.add('scrolled') : BODY.classList.remove('scrolled')
    currentScrollY < tempScrollPos
      ? BODY.classList.add('scrolling_top')
      : BODY.classList.remove('scrolling_top')

    currentScrollY > window.innerHeight
      ? BODY.classList.add('floatBind')
      : BODY.classList.remove('floatBind')

    tempScrollPos = currentScrollY
  })
}

export const inputControls = () => {
  var fields = document.querySelectorAll(
    'input:-webkit-autofill,input:not([type]),input[type=text]:not(.browser-default),input[type=password]:not(.browser-default),input[type=email]:not(.browser-default),input[type=url]:not(.browser-default),input[type=time]:not(.browser-default), input[type=date]:not(.browser-default), input[type=datetime]:not(.browser-default), input[type=datetime-local]:not(.browser-default), input[type=tel]:not(.browser-default), input[type=number]:not(.browser-default), input[type=search]:not(.browser-default), textarea'
  )

  if (!fields) return

  fields.forEach(function (el) {
    if (el.value.length) {
      el.closest('.input-field')?.classList.add('active')
    }

    el.addEventListener('focus', function () {
      el.closest('.input-field')?.classList.add('active')
    })

    el.addEventListener('blur', function () {
      if (el.value.length) {
        el.closest('.input-field')?.classList.add('active')
      } else {
        el.closest('.input-field')?.classList.remove('active')
      }
    })
  })
}

export const bodyVariables = () => {
  // body varibales
  setTimeout(() => {
    let hel = document.querySelector('header'),
      headerH = hel ? hel.clientHeight : 90,
      mobileMenu = document.querySelector('.mobileMenu'),
      mobileMenuH = mobileMenu ? mobileMenu.clientHeight : 55,
      topNav = document.querySelector('[data-topnav]'),
      topNavH = topNav ? topNav.clientHeight : 110
    let r = document.querySelector('body')

    if (mobileMenu) {
      r.style.setProperty('--mobileMenuHeight', mobileMenuH + 'px')
    }

    if (topNav) {
      r.style.setProperty('--topNavH', topNavH + 'px')
    }

    r.style.setProperty('--headerHeight', headerH + 'px')
  }, 100)
}

export const detectOSAndApplyClass = () => {
  const platform = navigator.platform
  let osClass = ''

  if (platform.indexOf('Win') !== -1) {
    osClass = 'os-windows'
  } else if (platform.indexOf('Mac') !== -1) {
    osClass = 'os-macos'
  } else if (platform.indexOf('Linux') !== -1) {
    osClass = 'os-linux'
  } else if (/(iPhone|iPad|iPod)/.test(platform)) {
    osClass = 'os-ios'
  } else if (/Android/.test(navigator.userAgent)) {
    osClass = 'os-android'
  }

  if (osClass) {
    document.body.classList.add(osClass)
  }
}

export const detectBrowserClass = () => {
  const userAgent = navigator.userAgent
  let browserClass = ''

  if (userAgent.indexOf('Chrome') !== -1 && userAgent.indexOf('Edg') === -1) {
    browserClass = 'browserChrome'
  } else if (userAgent.indexOf('Firefox') !== -1) {
    browserClass = 'browserFirefox'
  } else if (userAgent.indexOf('Safari') !== -1) {
    browserClass = 'browserSafari'
  } else if (userAgent.indexOf('Edg') !== -1) {
    browserClass = 'browserEdge'
  } else if (userAgent.indexOf('Trident') !== -1) {
    browserClass = 'browserIe'
  } else if (userAgent.indexOf('OPR') !== -1 || userAgent.indexOf('Opera') !== -1) {
    browserClass = 'browser-opera'
  }

  if (browserClass) {
    document.body.classList.add(browserClass)
  }
}

export const bodyProperty = () => {
  bodyVariables()
  addEventListener('resize', (event) => {
    bodyVariables()
  })

  const loaderTemp = document.querySelector('[data-loader]')
  setTimeout(() => BODY.classList.add('dom_loaded'), 50)

  if (loaderTemp) {
    // setTimeout(() => loaderTemp.remove(), 1800);
    setTimeout(() => {
      BODY.classList.add('dom_animStart')
    }, 50)
  }
}

export const scrollProgress = () => {
  var progressPath = document.querySelector('.progressWrap path')
  if (progressPath) {
    var pathLength = progressPath.getTotalLength()
    progressPath.style.transition = progressPath.style.WebkitTransition = 'none'
    progressPath.style.strokeDasharray = pathLength + ' ' + pathLength
    progressPath.style.strokeDashoffset = pathLength
    progressPath.getBoundingClientRect()
    progressPath.style.transition = progressPath.style.WebkitTransition =
      'stroke-dashoffset 10ms linear'
    var updateProgress = function () {
      var scroll = window.scrollY

      var html = document.documentElement

      var height = html.clientHeight - html.scrollHeight

      var progress = pathLength - (scroll * pathLength) / height
      progressPath.style.strokeDashoffset = progress
    }
    updateProgress()
    document.addEventListener('scroll', updateProgress)
    var offset = 50
    var duration = 550

    document.querySelector('.progressWrap').addEventListener('click', function (event) {
      event.preventDefault()
      if (window.lenis) window.lenis.scrollTo(0)

      return false
    })
  }
}

export const alertClick = () => {
  document.addEventListener('click', function (e) {
    // Check if the clicked element has the data-alertclose attribute
    if (e.target.closest('[data-alertclose]')) {
      // Find the button that was actually clicked (in case the SVG path was clicked)
      const closeButton = e.target.closest('[data-alertclose]')

      // Find the closest parent element with the class 'alert'
      const alertElement = closeButton.closest('.alert')

      // If an alert element is found, remove it from the DOM
      if (alertElement) {
        alertElement.remove()
      }
    }
  })
}

// int
webLoaded()
