var prices = db['Billing.Prices'];
prices.update({},{$set:{forPersonalAccount: false}}, {multi: true});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(20),
	monthsCount: NumberInt(12),
	price: 28,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(40),
	monthsCount: NumberInt(12),
	price: 55,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(80),
	monthsCount: NumberInt(12),
	price: 110,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(120),
	monthsCount: NumberInt(12),
	price: 165,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(200),
	monthsCount: NumberInt(12),
	price: 220,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(240),
	monthsCount: NumberInt(12),
	price: 265,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(280),
	monthsCount: NumberInt(12),
	price: 310,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(320),
	monthsCount: NumberInt(12),
	price: 350,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(360),
	monthsCount: NumberInt(12),
	price: 400,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(440),
	monthsCount: NumberInt(12),
	price: 440,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(480),
	monthsCount: NumberInt(12),
	price: 480,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(20),
	monthsCount: NumberInt(12),
	price: 0.6,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(40),
	monthsCount: NumberInt(12),
	price: 1.2,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(80),
	monthsCount: NumberInt(12),
	price: 2.2,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(120),
	monthsCount: NumberInt(12),
	price: 3.2,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(200),
	monthsCount: NumberInt(12),
	price: 4.5,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(240),
	monthsCount: NumberInt(12),
	price: 5.3,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(280),
	monthsCount: NumberInt(12),
	price: 6,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(320),
	monthsCount: NumberInt(12),
	price: 7,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(360),
	monthsCount: NumberInt(12),
	price: 8,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(440),
	monthsCount: NumberInt(12),
	price: 8.8,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(480),
	monthsCount: NumberInt(12),
	price: 9.6,
    forPersonalAccount: true
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(400),
	monthsCount: NumberInt(12),
	price: 550,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(2000),
	monthsCount: NumberInt(12),
	price: 2700,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(4000),
	monthsCount: NumberInt(12),
	price: 5450,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(6000),
	monthsCount: NumberInt(12),
	price: 8200,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(8000),
	monthsCount: NumberInt(12),
	price: 10900,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(10000),
	monthsCount: NumberInt(12),
	price: 13600,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(20000),
	monthsCount: NumberInt(12),
	price: 27300,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(30000),
	monthsCount: NumberInt(12),
	price: 31000,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'RUB',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(40000),
	monthsCount: NumberInt(12),
	price: 41500,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(400),
	monthsCount: NumberInt(12),
	price: 11,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(2000),
	monthsCount: NumberInt(12),
	price: 52,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(4000),
	monthsCount: NumberInt(12),
	price: 105,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(6000),
	monthsCount: NumberInt(12),
	price: 155,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(8000),
	monthsCount: NumberInt(12),
	price: 206,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(10000),
	monthsCount: NumberInt(12),
	price: 257,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(20000),
	monthsCount: NumberInt(12),
	price: 515,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(30000),
	monthsCount: NumberInt(12),
	price: 600,
    forPersonalAccount: false
});
prices.save({
	currencyType: 'USD',
	ventureId: 'SmartCAT',
	serviceType: NumberInt(1),
	amount: NumberInt(40000),
	monthsCount: NumberInt(12),
	price: 790,
    forPersonalAccount: false
});