// checkout-new.js
// Dependencies expected globally: axios, feather, notification
// Markup hooks expected:
//  - <form class="jsCheckoutForm"> ... </form>
//  - <button class="jsAddPayment" url="/Checkout/UpdatePayment">Add Payment</button>
//  - <div id="paymentBlock"> ... server-rendered partial ... </div>
//  - .loading-box (optional) for a page-level spinner
//  - .jsRemovePayment buttons inside the payments table (optional)

export default class CheckoutNew {
  constructor() {
    this.selectors = {
      form: '.jsCheckoutForm',
      addPaymentBtn: '.jsAddPayment',
      paymentBlock: '#paymentBlock',
      removePaymentBtn: '.jsRemovePayment',
      paymentMethodRadio: '.jsChangePayment',
      paymentMethodHost: '.jsPaymentMethod',
      antiForgery: 'input[name="__RequestVerificationToken"]',
      loadingBox: '.loading-box'
    };
  }

  // --- Utils ---
  showLoading(on = true) {
    const box = document.querySelector(this.selectors.loadingBox);
    if (!box) return;
    box.style.display = on ? 'block' : 'none';
  }

  getAntiForgeryHeader() {
    const token = document.querySelector(this.selectors.antiForgery)?.value;
    return token ? { 'RequestVerificationToken': token } : {};
  }

  replacePaymentBlock(html) {
    const host = document.querySelector(this.selectors.paymentBlock);
    if (!host) return;
    host.innerHTML = html;
    if (window.feather) feather.replace();
    // Re-bind dynamic handlers inside the refreshed block
    this.removePaymentClick();
    this.paymentMethodChange();
  }

  // --- Add Payment ---
  addPaymentClick() {
    const buttons = document.querySelectorAll(this.selectors.addPaymentBtn);
    if (!buttons.length) return;

    buttons.forEach((btn) => {
      btn.addEventListener('click', async () => {
        const url = btn.getAttribute('url');
        const form = document.querySelector(this.selectors.form);
        if (!url || !form) {
          console.warn('Missing url or .jsCheckoutForm for Add Payment.');
          return;
        }

        // Build FormData from the checkout form (matches server model binding)
        const data = new FormData(form);

        // If you also collect method from a selected radio, append it
        // (kept for parity with original file)
        const selectedMethod = document.querySelector(`${this.selectors.paymentMethodRadio}:checked`);
        if (selectedMethod) {
          data.append('PaymentMethodId', selectedMethod.getAttribute('methodid') || '');
          data.append('SystemKeyword', selectedMethod.getAttribute('keyword') || '');
        }

        // Anti-forgery: axios sends it as header even with FormData
        const headers = {
          'X-Requested-With': 'XMLHttpRequest',
          ...this.getAntiForgeryHeader()
        };

        try {
          this.showLoading(true);
          const res = await axios.post(url, data, { headers });
          if (res.status !== 200) {
            notification?.error(res);
            return;
          }
          // Server returns the updated payment block HTML (as in the original file)
          this.replacePaymentBlock(res.data);
        } catch (err) {
          if (err?.response?.status === 400) {
            // Optional: align with your UI
            const alert = document.querySelector('#giftcard-alert');
            if (alert) {
              alert.textContent = err.response.statusText || 'Bad Request';
              alert.classList.remove('alert-info');
              alert.classList.add('alert-danger');
            }
          } else {
            notification?.error(err);
          }
        } finally {
          this.showLoading(false);
        }
      });
    });
  }

  // --- Remove Payment (optional, if you render remove icons) ---
    removePaymentClick() {
        const removeBtns = document.querySelectorAll(this.selectors.removePaymentBtn);
        removeBtns.forEach((btn) => {
            // Avoid double-binding after partial replacement
            if (btn.dataset.bound === '1') return;
            btn.dataset.bound = '1';

            btn.addEventListener('click', async () => {
                const url = btn.getAttribute('data-url');
                const methodId = btn.getAttribute('data-method-id');  // Guid
                const keyword = btn.getAttribute('data-keyword');     // SystemKeyword

                if (!url || !methodId || !keyword) {
                    console.warn('Missing data-url / data-method-id / data-keyword on .jsRemovePayment');
                    return;
                }

                // Backend expects JSON body mapped to CheckoutViewModel
                const payload = {
                    systemKeyword: keyword,
                    paymentMethodId: methodId
                };

                try {
                    this.showLoading(true);
                    const res = await axios.post(url, payload, {
                        headers: {
                            'X-Requested-With': 'XMLHttpRequest',
                            'Content-Type': 'application/json; charset=UTF-8',
                            ...this.getAntiForgeryHeader() // harmless even though endpoint doesn't require it
                        }
                    });

                    // Server returns updated _AddPayment.cshtml HTML
                    this.replacePaymentBlock(res.data);
                } catch (err) {
                    notification?.error(err);
                } finally {
                    this.showLoading(false);
                }
            });
        });
    }

  // --- Payment Method change (optional radios) ---
  paymentMethodChange() {
    const radios = document.querySelectorAll(this.selectors.paymentMethodRadio);
    if (!radios.length) return;

    radios.forEach((r) => {
      // Avoid double-binding after partial replacement
      if (r.dataset.bound === '1') return;
      r.dataset.bound = '1';

      r.addEventListener('change', async () => {
        const url = r.getAttribute('url');
        const methodId = r.getAttribute('methodid');
        const keyword = r.getAttribute('keyword');
        if (!url) return;

        const data = new URLSearchParams();
        data.append('PaymentMethodId', methodId || '');
        data.append('SystemKeyword', keyword || '');

        try {
          this.showLoading(true);
          const res = await axios.post(url, data, {
            headers: {
              'X-Requested-With': 'XMLHttpRequest',
              'Content-Type': 'application/x-www-form-urlencoded; charset=UTF-8',
              ...this.getAntiForgeryHeader()
            }
          });

          // Replace only the method host content if you render a sub-partial
          const host = document.querySelector(this.selectors.paymentMethodHost);
          if (host) host.innerHTML = res.data;
          if (window.feather) feather.replace();
        } catch (err) {
          notification?.error(err);
        } finally {
          this.showLoading(false);
        }
      });
    });
  }

  init() {
    this.addPaymentClick();
    this.removePaymentClick();
    this.paymentMethodChange();
  }
}

// Usage example:
// import CheckoutNew from './checkout-new';
// const checkout = new CheckoutNew();
// checkout.init();
